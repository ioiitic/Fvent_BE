using Fvent.BO.Enums;

namespace Fvent.Service.Request;

public record AuthReq(string Email, string Password);

public record RefreshTokenReq(string Token);

public record GetListUsersReq(string? Username, string? Email, string? RoleName, bool? Verified, string? OrderBy,
                              bool IsDescending = false, int PageNumber = 1, int PageSize = 9);

public record CreateUserReq(string Username, string Email, string Password, string FirstName, string LastName,
                            string PhoneNumber, string Role);

public record UpdateUserReq(string? Username, string? AvatarUrl, string? Email, string? FirstName,
                            string? LastName, string? PhoneNumber);

public record UpdateUserCardReq(string CardUrl);

public record ForgotPasswordReq(string Email);
public record ChangePasswordRequest(string OldPassword, string NewPassword);


//public record ResetPasswordReq(Guid userId, string token, string newPassword);
