namespace Fvent.Service.Request;

public record CreateUserReq(string Username,
                            string AvatarUrl,
                            string Email,
                            string Password,
                            string FirstName,
                            string LastName,
                            string PhoneNumber,
                            string CardUrl,
                            string Campus,
                            int RoleId);

public record UpdateUserReq(string Username,
                            string AvatarUrl,
                            string Email,
                            string Password,
                            string FirstName,
                            string LastName,
                            string PhoneNumber,
                            string CardUrl,
                            string Campus,
                            int RoleId);