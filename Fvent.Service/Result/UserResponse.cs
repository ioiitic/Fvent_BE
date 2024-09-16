namespace Fvent.Service.Result;

public record UserRes(string Username,
                      string AvatarUrl,
                      string Email,
                      string Password,
                      string FirstName,
                      string LastName,
                      string PhoneNumber,
                      string CardUrl,
                      string Campus,
                      string RoleName);
