using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Microsoft.Extensions.Configuration;
using static Fvent.Service.Specifications.NotificationSpec;
using static Fvent.Service.Specifications.UserSpec;
using JS = Fvent.Service.Utils.JwtService;
using HS = Fvent.Service.Utils.HashService;
using static Fvent.Service.Specifications.EventSpec;

namespace Fvent.Service.Services.Imp;

public class UserService(IUnitOfWork uOW, IConfiguration configuration, IEmailService emailService) : IUserService
{
    #region Authen
    public async Task<AuthRes> Authen(AuthReq req, string ipAddress)
    { 
        req = new AuthReq(req.Email, HS.ToSHA256(req.Password), req.FcmToken);
        // Check user authen
        var spec = new AuthenUserSpec(req.Email, req.Password);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Generate jwt token and refresh token
        var token = JS.GenerateToken(user.UserId, user.Email, user.Role!, configuration);
        var rfsToken = CreateRefreshToken(user, ipAddress);

        if(req.FcmToken is not null)
        {
            user.FcmToken = req.FcmToken;
        }
        await uOW.SaveChangesAsync();

        return new AuthRes(token, rfsToken.Token);
    }

    public async Task<AuthRes> Refresh(RefreshTokenReq req, string ipAddress)
    {
        // Check if the refresh token exists
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

        // Load the associated user
        var user = rfsToken.User ?? throw new Exception("Associated user not found for the refresh token.");

        // Ensure RefreshTokens is initialized
        user.RefreshTokens ??= new List<RefreshToken>();

        // Generate JWT token and refresh token
        var token = JS.GenerateToken(user.UserId, user.Email, user.Role ?? throw new Exception("User role is missing"), configuration);
        var newRefreshToken = CreateRefreshToken(user, ipAddress);

        await uOW.SaveChangesAsync();

        return new AuthRes(token, newRefreshToken.Token);
    }
    #endregion

    #region User
    public async Task<PageResult<GetListUserRes>> GetList(GetListUsersReq req)
    {
        var spec = new GetListUsersSpec(req.Username, req.Email, req.RoleName, req.Verified, req.OrderBy,
                                        req.IsDescending, req.PageNumber, req.PageSize);
        var users = await uOW.Users.GetPageAsync(spec);

        return new PageResult<GetListUserRes>(users.Items.Select(u => u.ToResponse<GetListUserRes>()), users.PageNumber,
                                              users.PageSize, users.Count, users.TotalItems, users.TotalPages);
    }

    public async Task<PageResult<GetListUserRes>> GetListBannedUser(GetListUsersReq req)
    {
        var spec = new GetListBannedUsersSpec(req.Username, req.Email, req.RoleName, req.Verified, req.OrderBy,
                                        req.IsDescending, req.PageNumber, req.PageSize).SetIgnoreQueryFilters(true);

        var users = await uOW.Users.GetPageAsync(spec);

        return new PageResult<GetListUserRes>(users.Items.Select(u => u.ToResponse<GetListUserRes>()), users.PageNumber,
                                              users.PageSize, users.Count, users.TotalItems, users.TotalPages);
    }

    public async Task<UserRes> Get(Guid id)
    {
        var user = await _Get(id);

        // Check if user has notifications
        var specSub = new GetUnreadNotificationByUserSpec(id);
        var isHaveUnreadNoti = await uOW.Notification.FindFirstOrDefaultAsync(specSub) != null;

        return user.ToResponse<UserRes>(isHaveUnreadNoti);
    }

    public async Task<IdRes> Update(Guid id, UpdateUserReq req)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Check for unique StudentId
        var existingStudent = await uOW.Users.FindFirstOrDefaultAsync(new GetUserByStudentIdSpec(req.StudentId));
        if (existingStudent != null && existingStudent.UserId != user.UserId)
        {
            throw new Exception($"Student ID {req.StudentId} is already in use");
        }

        user.Update(req.Username,
            req.AvatarUrl,
            req.PhoneNumber,
            req.StudentId);

        if (uOW.IsUpdate(user))
            user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }

    public async Task Delete(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        uOW.Users.Delete(user);
        // Send verification email
        var emailBody = EmailTemplates.PermanentAccountDeletionTemplate.Replace("{userName}", user.Username);
        await emailService.SendEmailAsync(user.Email, "Thông Báo Về Việc Khóa Tài Khoản", emailBody);

        await uOW.SaveChangesAsync();
    }

    public async Task UnBan(Guid id)
    {
        var spec = new GetUserSpec(id).SetIgnoreQueryFilters(true);

        // Find the user with the specified ID
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Check if the user is actually soft-deleted
        if (!user.IsDeleted)
        {
            throw new InvalidOperationException("The specified user is not deleted.");
        }
        user.IsDeleted = false;
        user.DeletedAt = null;

        // Send verification email
        var emailBody = EmailTemplates.UnbanUserNotificationTemplate.Replace("{userName}", user.Username).Replace("{loginLink}", "https://fvent.example.com/login");
        await emailService.SendEmailAsync(user.Email, "Thông Báo Gỡ Khóa Tài Khoản", emailBody);

        // Save the changes
        await uOW.SaveChangesAsync();
    }

    #endregion

    #region User Account
    public async Task<IdRes> Register(CreateUserReq req)
    {
        req = new CreateUserReq(req.Username, req.Email, HS.ToSHA256(req.Password), req.StudentId, req.PhoneNumber, req.Role);
        var spec = new GetUserSpec(req.Email).SetIgnoreQueryFilters(true);
        var existingUser = await uOW.Users.FindFirstOrDefaultAsync(spec);

        var studentId = req.StudentId;

        if (existingUser != null)
        {
            if (existingUser.EmailVerified)
            {
                throw new ValidationException("This email has been used");
            }

            // Update user info
            existingUser.Update(req.Username, req.Password, req.PhoneNumber, req.StudentId, req.Role);
        }
        else
        {
            // Create a new user
            existingUser = req.ToUser();

            // Check if the email matches @fpt.edu.vn pattern
            if (req.Email.EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
            {
                // Extract the 8-character Student ID
                studentId = req.Email.Split('@')[0];
                studentId = studentId.Length >= 8 ? studentId[^8..].ToUpper() : studentId.ToUpper();

                // Assign VerifiedStatus.Verified and set StudentId
                existingUser.Verified = VerifiedStatus.Verified;
                existingUser.StudentId = studentId;
            }

            await uOW.Users.AddAsync(existingUser);
        }

        // Check for duplicate studentId
        var existingStudent = await uOW.Users.FindFirstOrDefaultAsync(new GetUserByStudentIdSpec(studentId));
        if (existingStudent != null)
        {
            throw new Exception($"Student ID {req.StudentId} is already in use");
        }

        // Generate and save verification token
        var token = Guid.NewGuid().ToString();
        var verificationLink = GenerateVerificationLink(existingUser.UserId, token);

        var verificationToken = new VerificationToken(existingUser.UserId, token);
        if (existingUser.VerificationToken is null)
        {
            existingUser.VerificationToken = verificationToken;
        }
        else
        {
            existingUser.VerificationToken.Token = verificationToken.Token;
            existingUser.VerificationToken.ExpiryDate = verificationToken.ExpiryDate;
        }
        //await uOW.VerificationToken.AddAsync(verificationToken);

        await uOW.SaveChangesAsync();

        // Send verification email
        var emailBody = EmailTemplates.EmailVerificationTemplate.Replace("{verificationLink}", verificationLink);
        await emailService.SendEmailAsync(req.Email, "Xác Nhận Email", emailBody);

        return existingUser.UserId.ToResponse();
    }

    public async Task<IdRes> AddCardId(Guid id, string cardUrl)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Check for unique StudentId
        var existingStudent = await uOW.Users.FindFirstOrDefaultAsync(new GetUserByStudentIdSpec(user.StudentId));
        if (existingStudent != null)
        {
            throw new Exception($"Student ID {user.StudentId} is already in use");
        }

        user.CardUrl = cardUrl;
        user.Verified = VerifiedStatus.UnderVerify;
        user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }

    public async Task<IdRes> ApproveUser(Guid id, bool isApproved, string processNote)
    {
        var serviceKeyPath = Path.Combine(AppContext.BaseDirectory, "firebase-service-key.json");
        var firebaseService = new FirebaseService(serviceKeyPath);
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        if (isApproved)
        {
            user.Verified = VerifiedStatus.Verified;

            var notificationReq = new CreateNotificationReq(
                user.UserId,
                null, 
                "Xác minh tài khoản thành công!",
                "Chúc mừng! Tài khoản của bạn đã được xác minh thành công. Giờ đây, bạn có thể sử dụng đầy đủ các tính năng của hệ thống. Hãy bắt đầu khám phá ngay nhé!" 
            );

            if(user.FcmToken is not null)
            {
                // Send a single notification to the user
                await firebaseService.SendNotificationAsync(
                    user.FcmToken,
                   "Xác minh tài khoản thành công!",
                   "Chúc mừng! Tài khoản của bạn đã được xác minh thành công. Giờ đây, bạn có thể sử dụng đầy đủ các tính năng của hệ thống. Hãy bắt đầu khám phá ngay nhé!"
                );
            }

            var notification = notificationReq.ToNotification();
            await uOW.Notification.AddAsync(notification);

        }
        else
        {
            user.Verified = VerifiedStatus.Rejected;

            var notificationReq = new CreateNotificationReq(
                user.UserId,
                null,
                "Yêu cầu xác minh tài khoản bị từ chối",
                "Rất tiếc! Yêu cầu xác minh tài khoản của bạn đã bị từ chối. Vui lòng kiểm tra lại thông tin và gửi yêu cầu xác minh mới nếu cần thiết." 
            );

            if (user.FcmToken is not null)
            {
                // Send a single notification to the user
                await firebaseService.SendNotificationAsync(
                    user.FcmToken,
                    "Yêu cầu xác minh tài khoản bị từ chối",
                    "Rất tiếc! Yêu cầu xác minh tài khoản của bạn đã bị từ chối. Vui lòng kiểm tra lại thông tin và gửi yêu cầu xác minh mới nếu cần thiết."
                );
            }

            var notification = notificationReq.ToNotification();
            await uOW.Notification.AddAsync(notification);

        }
        user.ProcessNote = processNote;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }

    public async Task<IdRes> RegisterModerator(CreateModeratReq req)
    {
        var existingUserSpec = new GetUserSpec(req.Email, "moderator");
        var existingUser = await uOW.Users.FindFirstOrDefaultAsync(existingUserSpec);

        if (existingUser != null)
        {
            throw new Exception("The email is already in use. Please choose a different email.");
        }

        // Convert the request to a User entity
        var moderator = req.ToModerator();

        // Add the moderator user to the database and save
        await uOW.Users.AddAsync(moderator);
        await uOW.SaveChangesAsync();

        // Send a notification email with initial login instructions (optional)
        var emailBody = EmailTemplates.ModeratorWelcomeTemplate.Replace("{moderatorName}", moderator.Username)
                                                               .Replace("{loginLink}", "https://fvent.example.com/login");

        await emailService.SendEmailAsync(moderator.Email, "Chào mừng đến Fvent!", emailBody);

        return moderator.UserId.ToResponse();
    }
    #endregion

    #region Email
    public async Task VerifyEmailAsync(Guid userId, string token)
    {
        var userSpec = new GetUserSpec(userId).SetIgnoreQueryFilters(true);
        var user = await uOW.Users.FindFirstOrDefaultAsync(userSpec)
            ?? throw new NotFoundException(typeof(User));

        if (user.VerificationToken is null)
            throw new NotFoundException(typeof(VerificationToken));

        user.EmailVerified = true;
        uOW.VerificationToken.Delete(user.VerificationToken);
        await uOW.SaveChangesAsync();

        return;
    }

    public async Task RequestPasswordResetAsync(string email)
    {
        // Step 1: Find user by email
        var spec = new GetUserSpec(email);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Step 2: Generate or replace a reset token
        var storedToken = await uOW.VerificationToken.FindFirstOrDefaultAsync(new GetVerificationTokenSpec(user.UserId));

        if (storedToken != null)
        {
            uOW.VerificationToken.Delete(storedToken); 
        }

        var token = Guid.NewGuid().ToString();
        var resetToken = new VerificationToken(user.UserId, token, DateTime.Now.AddHours(14));

        // Step 3: Store token in the database
        await uOW.VerificationToken.AddAsync(resetToken);
        await uOW.SaveChangesAsync();

        // Step 4: Generate the reset link
        var resetLink = GenerateResetLink(user.UserId, token);

        // Step 5: Load email template and send email
        var emailBody = EmailTemplates.PasswordResetTemplate.Replace("{resetLink}", resetLink).Replace("{userName}", user.Username);

        await emailService.SendEmailAsync(user.Email, "Yêu Cầu Đặt Lại Mật Khẩu", emailBody);
    }

    public async Task ResetPasswordAsync(Guid userId, string token, string newPassword)
    {
        // Step 1: Verify the token
        var spec = new GetVerificationTokenSpec(userId, token);
        var resetToken = await uOW.VerificationToken.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(VerificationToken));

        // Step 2: Check if token is expired
        if (resetToken.ExpiryDate < DateTime.Now.AddHours(13))
        {
            throw new InvalidOperationException("Reset token has expired.");
        }

        // Step 3: Find the user
        var user = await uOW.Users.FindFirstOrDefaultAsync(new GetUserSpec(userId))
            ?? throw new NotFoundException(typeof(User));

        // Step 4: Update user's password
        user.Password = HS.ToSHA256(newPassword);
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
        if (user.Password.CompareTo(HS.ToSHA256(oldPassword)) != 0)
        {
            throw new UnauthorizedAccessException("Old password is incorrect.");
        }

        user.Password = HS.ToSHA256(newPassword);
        await uOW.SaveChangesAsync();
    }

    public async Task<IdRes> ResendVerificationEmail(string userEmail, string role)
    {
        var spec = new GetUserSpec(userEmail, role);


        // Find the user by email
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec);

        if (user == null || user.EmailVerified)
        {
            throw new Exception("User does not exist or is already verified.");
        }

        // Generate a new token
        var token = Guid.NewGuid().ToString();
        var verificationLink = GenerateVerificationLink(user.UserId, token);

        // Check if there's an existing verification token for this user
        var specSub = new GetVerificationTokenSpec(user.UserId);
        var existingToken = await uOW.VerificationToken.FindFirstOrDefaultAsync(specSub);


        if (existingToken != null)
        {
            // Update the existing token
            existingToken.Token = token;
            existingToken.ExpiryDate = DateTime.Now.AddHours(24);
        }
        else
        {
            // Add a new verification token if none exists
            var verificationToken = new VerificationToken(user.UserId, token);
            await uOW.VerificationToken.AddAsync(verificationToken);
        }

        // Save changes to update or add the token
        await uOW.SaveChangesAsync();

        // Send the verification email
        var emailBody = EmailTemplates.EmailVerificationTemplate.Replace("{verificationLink}", verificationLink);
        await emailService.SendEmailAsync(user.Email, "Email Verification - Resend", emailBody);

        return user.UserId.ToResponse();
    }
    #endregion

    #region Report
    public async Task<UserReportRes> GetReport(Guid userId)
    {
        var spec = new GetRegisteredEventsSpec(userId);
        var events = await uOW.Events.GetListAsync(spec);

        var noOfEvents = events.Count();
        var organizersList = events
            .GroupBy(e => e.OrganizerId)
            .Distinct()
            .Select(e => new
            {   
                OrganizerId = e.Key,
                EventCount = e.Count(),
                CheckInCount = e.Count(ed => ed.Registrations!.Any(r => r.EventId == ed.EventId && r.IsCheckIn))
            })
            .OrderByDescending(o => o.EventCount)
            .Take(5);

        var organizers = new List<OrganizerReportInfo>();
        foreach (var organizer in organizersList)
        {
            var user = await _Get(organizer.OrganizerId);
            organizers.Add(user.ToResponse<OrganizerReportInfo>(noOfEvent: organizer.EventCount, noOfCheckIn: organizer.CheckInCount));
        }

        return new UserReportRes(noOfEvents, organizersList.Count(), organizers.Sum(o => o.NoOfCheckIn), organizers);
    }
    #endregion

    #region Private function
    private RefreshToken CreateRefreshToken(User user, string ipAddress)
    {
        var rfsToken = JS.GenerateRefreshToken(ipAddress);

        // Add refresh token
        user.RefreshTokens!.Add(rfsToken);

        return rfsToken;
    }

    private async Task<User> _Get(Guid id)
    {
        // Fetch user using specification
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        // Check if user has notifications
        var specSub = new GetUnreadNotificationByUserSpec(id);
        var isHaveUnreadNoti = await uOW.Notification.FindFirstOrDefaultAsync(specSub) != null;

        // Map user entity to response object, passing isHaveNoti
        return user;
    }

    private string GenerateVerificationLink(Guid userId, string token)
    {
        return $"https://fvent.vercel.app/xac-thuc-email?userId={userId}&token={token}";
        //return $"https://fvent.somee.com/api/users/verify-email?userId={userId}&token={token}";
    }

    private string GenerateResetLink(Guid userId, string token)
    {
        return $"https://fvent.vercel.app/dat-lai-mat-khau?userId={userId}&token={token}";
        //return $"https://fvent.somee.com/api/users/reset-password?userId={userId}&token={token}";
    }
    #endregion
}