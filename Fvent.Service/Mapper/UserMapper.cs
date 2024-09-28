using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;

public static class UserMapper
{
    public static User ToUser(this CreateUserReq src)
    {
        // Try to parse the role from the string to the UserRole enum
        if (!Enum.TryParse<UserRole>(src.Role, true, out var userRole))
        {
            throw new ArgumentException("Invalid role specified"); // Handle invalid role
        }

        return new User(
            src.Username,
            "",
            src.Email,
            src.Password,
            src.FirstName,
            src.LastName,
            src.PhoneNumber,
            "", 
            "HCM", 
            (int)userRole,
            DateTime.UtcNow
        );
    }

    public static UserRes ToReponse(
        this User src,
        string roleName)
        => new (
            src.Username,
            src.AvatarUrl,
            src.Email,
            src.Password,
            src.FirstName,
            src.LastName,
            src.PhoneNumber,
            src.CardUrl,
            src.Campus,
            roleName);
}
