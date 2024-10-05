namespace Fvent.Service.Result;

public record GetListUserRes(string Username, string AvatarUrl, string Email, string FirstName, string LastName,
                                  string PhoneNumber, string CardUrl, bool Verified, string RoleName, DateTime CreatedAt,
                                  DateTime? UpdatedAt, bool IsDeleted, DateTime? DeletedAt);

public record UserRes(Guid UserId, string Username, string AvatarUrl, string Email, string Password, string FirstName,
                      string LastName, string PhoneNumber, string CardUrl, string RoleName);

public record AuthResponse(string Token);