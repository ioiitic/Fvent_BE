namespace Fvent.Service.Result;

public record GetListUserRes(string Username, string AvatarUrl, string Email,
                             string PhoneNumber, string CardUrl, VerifiedStatus Verified, string RoleName, DateTime CreatedAt,
                             DateTime? UpdatedAt, bool IsDeleted, DateTime? DeletedAt);

public record UserRes(Guid UserId, string Username, string AvatarUrl, string Email, string PhoneNumber, string CardUrl, string VerifyStatus,string ProcessNote, string RoleName);

public record AuthResponse(string Token, string RefreshToken);