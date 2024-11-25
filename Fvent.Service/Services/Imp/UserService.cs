﻿using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using LinqKit;
using Microsoft.Extensions.Configuration;
using static Fvent.Service.Specifications.UserSpec;
using JS = Fvent.Service.Utils.JwtService;

namespace Fvent.Service.Services.Imp;

public class UserService(IUnitOfWork uOW, IConfiguration configuration, IEmailService emailService, IEventService eventService) : IUserService
{
    #region Authen
    public async Task<AuthRes> Authen(AuthReq req, string ipAddress)
    { 
        // Check user authen
        var spec = new AuthenUserSpec(req.Email, req.Password);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Generate jwt token and refresh token
        var token = JS.GenerateToken(user.UserId, user.Email, user.Role!, configuration);
        var rfsToken = await CreateRefreshToken(user, ipAddress);

        return new AuthRes(token, rfsToken.Token);
    }

    public async Task<AuthRes> Refresh(RefreshTokenReq req, string ipAddress)
    {
        // Check refresh toen exist
        var rfsSpec = new CheckRefreshTokenSpec(req.Token);
        var rfsToken = await uOW.RefreshToken.FindFirstOrDefaultAsync(rfsSpec) 
            ?? throw new NotFoundException(typeof(RefreshToken));

        // Validate token
        if (rfsToken.IsRevoked)
        {
            throw new AuthenticationException("Refresh token is revoked!");
        }
        if (rfsToken.IsExpired)
        {
            throw new AuthenticationException("Refresh token is expired!");
        }

        // Generate jwt token and refresh token
        var user = rfsToken.User!;
        var token = JS.GenerateToken(user.UserId, user.Email, user.Role!, configuration);
        rfsToken = await CreateRefreshToken(user, ipAddress);

        return new AuthRes(token, rfsToken.Token);
    }

    private async Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        var rfsToken = JS.GenerateRefreshToken(ipAddress);
        var rfsSpec = new CheckRefreshTokenSpec(rfsToken.Token);
        while ((await uOW.RefreshToken.FindFirstOrDefaultAsync(rfsSpec)) != null)
        {
            rfsToken = JS.GenerateRefreshToken(ipAddress);
        }

        // Replace refresh token
        user.RefreshTokens!.Add(rfsToken);
        uOW.SaveChanges();

        return rfsToken;
    }
    #endregion

    #region Email
    public async Task<UserRes> GetByEmail(string email)
    {
        var spec = new GetUserSpec(email);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user.ToResponse<UserRes>();
    }

    #endregion

    #region User
    public async Task<IdRes> Update(Guid id, UpdateUserReq req)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        user.Update(req.Username,
            req.AvatarUrl,
            req.PhoneNumber);

        if (uOW.IsUpdate(user))
            user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }
    #endregion
    #region Admin
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
        user.Verified = VerifiedStatus.Unverified;

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

    public async Task<IdRes> AddCardId(Guid id, string cardUrl)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        user.CardUrl = cardUrl;
        user.Verified = VerifiedStatus.UnderVerify;
        user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }

    public async Task<IdRes> ApproveUser(Guid id, bool isApproved, string processNote)
    {
        var spec = new GetUserSpec(id);
        var _user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));
        if (isApproved)
        {
            _user.Verified = VerifiedStatus.Verified;
        }
        else
        {
            _user.Verified = VerifiedStatus.Rejected;
        }

        _user.ProcessNote = processNote;

        await uOW.SaveChangesAsync();

        return _user.UserId.ToResponse();
    }

    public async Task VerifyEmailAsync(Guid userId, string token)
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

        return;
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

    public async Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await uOW.Users.FindFirstOrDefaultAsync(new GetUserSpec(userId))
            ?? throw new NotFoundException(typeof(User));

        // Verify the old password
        if (user.Password.CompareTo(oldPassword)!=0)
        {
            throw new UnauthorizedAccessException("Old password is incorrect.");
        }

        user.Password = newPassword;
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
        var user = await _Get(id);

        return user.ToResponse<UserRes>();
    }
    private async Task<User> _Get(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user;
    }

    private string GenerateVerificationLink(Guid userId, string token)
    {
        //return $"https://localhost:7289/api/users/verify-email?userId={userId}&token={token}";
        return $"https://fvent.somee.com/api/users/verify-email?userId={userId}&token={token}";

    }
    private string GenerateResetLink(Guid userId, string token)
    {
        //return $"https://localhost:7289/api/users/reset-password?userId={userId}&token={token}";
        return $"https://fvent.somee.com/api/users/reset-password?userId={userId}&token={token}";
    }

    #region Report
    public async Task<UserReportRes> GetReport(Guid userId)
    {
        var events = await eventService.GetRegisteredEvents(userId, true);

        var noOfEvents = events.Count();
        var organizersList = events
            .GroupBy(e => e.OrganizerId)
            .Distinct()
            .Select(e => new
            {   
                OrganizerId = e.Key,
                EventCount = e.Count()
            })
            .OrderByDescending(o => o.EventCount)
            .Take(5);   

        var organizers = new List<OrganizerReportInfo>();
        foreach (var organizer in organizersList)
        {
            var user = await _Get(organizer.OrganizerId);
            organizers.Add(user.ToResponse<OrganizerReportInfo>(organizer.EventCount));
        }

        return new UserReportRes(noOfEvents, organizersList.Count(), organizers);
    }
    #endregion
}
