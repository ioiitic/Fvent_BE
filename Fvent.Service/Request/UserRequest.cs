using Fvent.BO.Enums;

namespace Fvent.Service.Request;

#region User
public record AuthReq(string Email, string Password);
#endregion

#region Admin
public record GetListUsersReq(string? Username, string? Email, string? RoleName, bool? Verified);
#endregion

public record CreateUserReq(string Username, string Email, string Password, string FirstName, string LastName,
                            string PhoneNumber, string Role);

public record UpdateUserReq(string Username, string AvatarUrl, string Email, string Password, string FirstName,
                            string LastName, string PhoneNumber, string CardUrl, string Campus, int RoleId);
