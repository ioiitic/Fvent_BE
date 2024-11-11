using Fvent.BO.Common;
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
            DefaultImage.DefaultAvatar,
            src.Email,
            src.Password,
            src.PhoneNumber,
            "", 
            (int)userRole,
            DateTime.UtcNow
        );
    }

    public static TEntity ToResponse<TEntity>(this User src) where TEntity : class
    {
        var result = typeof(TEntity) switch
        {
            Type t when t == typeof(UserRes) =>
                new UserRes(src.UserId, src.Username, src.AvatarUrl, src.Email,
                            src.PhoneNumber, src.CardUrl, src.Role!.RoleName) as TEntity,

            Type t when t == typeof(GetListUserRes) =>
                new GetListUserRes(src.Username, src.AvatarUrl, src.Email,
                                        src.PhoneNumber, src.CardUrl, src.Verified, src.Role!.RoleName, src.CreatedAt,
                                        src.UpdatedAt, src.IsDeleted, src.DeletedAt) as TEntity,

            _ => throw new InvalidOperationException($"Unsupported type: {typeof(TEntity).Name}")
        };

        if (result is null)
        {
            throw new InvalidOperationException($"Failed to cast to {typeof(TEntity).Name}");
        }

        return result;
    }
}
