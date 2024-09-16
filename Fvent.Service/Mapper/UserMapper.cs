using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;

public static class UserMapper
{
    public static User ToUser(
        this CreateUserReq src)
        => new(
            src.Username,
            src.AvatarUrl,
            src.Email,
            src.Password,
            src.FirstName,
            src.LastName,
            src.PhoneNumber,
            src.CardUrl,
            src.Campus,
            src.RoleId,
            DateTime.UtcNow);

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
