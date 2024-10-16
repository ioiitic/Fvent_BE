using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Microsoft.Extensions.Configuration;
using static Fvent.Service.Specifications.UserSpec;
using JS = Fvent.Service.Utils.JwtService;

namespace Fvent.Service.Services.Imp;

public class UserService(IUnitOfWork uOW, IConfiguration configuration, IEmailService emailService) : IUserService
{
    #region User
    /// <summary>
    /// Implement service for User Login
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<AuthResponse> Authen(AuthReq req)
    {
        var spec = new AuthenUserSpec(req.Email, req.Password);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        var token = JS.GenerateToken(user.Email, user.Role!, configuration);

        return new AuthResponse(token);
    }

    /// <summary>
    /// Implement service for User Get own info
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserRes> GetByEmail(string email)
    {
        var spec = new GetUserSpec(email);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user.ToResponse<UserRes>();
    }

    /// <summary>
    /// Implement service for User Update info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<IdRes> Update(Guid id, UpdateUserReq req)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        user.Update(req.Username,
            req.AvatarUrl,
            req.Email,
            req.Password,
            req.FirstName,
            req.LastName,
            req.PhoneNumber,
            req.CardUrl);

        if (uOW.IsUpdate(user))
            user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }
    #endregion

    #region Admin
    /// <summary>
    /// Service for Admin Get list users info
    /// </summary>
    /// <returns></returns>
    public async Task<PageResult<GetListUserRes>> GetList(GetListUsersReq req)
    {
        var spec = new GetListUsersSpec(req.Username, req.Email, req.RoleName, req.Verified, req.OrderBy,
                                        req.IsDescending, req.PageNumber, req.PageSize);
        var users = await uOW.Users.GetPageAsync(spec);

        return new PageResult<GetListUserRes>(users.Items.Select(u => u.ToResponse<GetListUserRes>()), users.PageNumber,
                                              users.PageSize, users.Count, users.TotalItems, users.TotalPages);
    }
    #endregion

    public async Task<IdRes> Register(CreateUserReq req)
    {
        var user = req.ToUser();
        user.EmailVerified = false;

        await uOW.Users.AddAsync(user);
        await uOW.SaveChangesAsync();

        var token = Guid.NewGuid().ToString();
        var verificationLink = GenerateVerificationLink(user.UserId, token);

        var verificationToken = new VerificationToken(user.UserId, token);
        await uOW.VerificationToken.AddAsync(verificationToken);
        await uOW.SaveChangesAsync();

        // Send the verification email using Gmail SMTP
 
        var emailBody = EmailTemplates.EmailVerificationTemplate.Replace("{verificationLink}", verificationLink);

        await emailService.SendEmailAsync(user.Email, "Email Verification", emailBody);

        return user.UserId.ToResponse();
    }

    public async Task<bool> VerifyEmailAsync(Guid userId, string token)
    {
        // Step 1: Check if the token exists in the database
        var spec = new GetVerificationTokenSpec(userId, token);
        var storedToken = await uOW.VerificationToken.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(VerificationToken));

        // Step 2: Verify the user
        var userSpec = new GetUserSpec(userId).SetIgnoreQueryFilters(true);
        var user = await uOW.Users.FindFirstOrDefaultAsync(userSpec)
            ?? throw new NotFoundException(typeof(User));

        user.EmailVerified = true;

        // Save changes to the user
        await uOW.SaveChangesAsync();

        // Step 3: Delete the token after successful verification
        uOW.VerificationToken.Delete(storedToken);
        await uOW.SaveChangesAsync();

        return true;
    }

    public async Task RequestPasswordResetAsync(string email)
    {
        // Step 1: Find user by email
        var user = await uOW.Users.FindFirstOrDefaultAsync(new GetUserSpec(email))
            ?? throw new NotFoundException(typeof(User));

        // Step 2: Generate or replace a reset token
        var storedToken = await uOW.VerificationToken.FindFirstOrDefaultAsync(new GetVerificationTokenSpec(user.UserId));

        if (storedToken != null)
        {
            uOW.VerificationToken.Delete(storedToken); // Remove the existing token before generating a new one
        }


        var token = Guid.NewGuid().ToString();
        var resetToken = new VerificationToken(user.UserId, token, DateTime.UtcNow.AddHours(1));

        // Step 3: Store token in the database
        await uOW.VerificationToken.AddAsync(resetToken);
        await uOW.SaveChangesAsync();

        // Step 4: Generate the reset link
        var resetLink = GenerateResetLink(user.UserId, token);

        // Step 5: Load email template and send email
        var emailBody = EmailTemplates.PasswordResetTemplate.Replace("{resetLink}", resetLink);

        await emailService.SendEmailAsync(user.Email, "Reset Your Password", emailBody);
    }

    public async Task ResetPasswordAsync(Guid userId, string token, string newPassword)
    {
        // Step 1: Verify the token
        var spec = new GetVerificationTokenSpec(userId, token);
        var resetToken = await uOW.VerificationToken.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(VerificationToken));

        // Step 2: Check if token is expired
        if (resetToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Reset token has expired.");
        }

        // Step 3: Find the user
        var user = await uOW.Users.FindFirstOrDefaultAsync(new GetUserSpec(userId))
            ?? throw new NotFoundException(typeof(User));

        // Step 4: Update user's password
        user.Password = newPassword;
        await uOW.SaveChangesAsync();

        // Step 5: Remove the token after successful reset
        uOW.VerificationToken.Delete(resetToken);
        await uOW.SaveChangesAsync();
    }



    public async Task Delete(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        uOW.Users.Delete(user);

        await uOW.SaveChangesAsync();
    }

    public async Task<UserRes> Get(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user.ToResponse<UserRes>();
    }

    private string GenerateVerificationLink(Guid userId, string token)
    {
        return $"https://localhost:7289/api/users/verify-email?userId={userId}&token={token}";
    }

    private string GenerateResetLink(Guid userId, string token)
    {
        return $"https://localhost:7289/api/users/reset-password?userId={userId}&token={token}";
    }

}
