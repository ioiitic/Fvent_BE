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
        return new User(
            src.Username,
            DefaultImage.DefaultAvatar,
            src.Email,
            src.StudentId,
            src.Password,
            "",
            "",
            src.PhoneNumber,
            "", 
            (int)src.Role,
            DateTime.Now.AddHours(13),
            false,
            VerifiedStatus.Unverified
        );
    }

    public static User ToModerator(this CreateModeratReq src)
    {
        return new User(
            src.Username,
            DefaultImage.DefaultAvatar,
            src.Email,
            "",
            src.Password,
            "",
            "",
            "",
            "",
            (int)UserRole.Moderator,
            DateTime.Now.AddHours(13),
            true,
            VerifiedStatus.Verified
        );
    }

    public static TEntity ToResponse<TEntity>(this User src, bool isHaveUnreadNoti = false, int? noOfEvent = null) where TEntity : class
    {
        var result = typeof(TEntity) switch
        {
            Type t when t == typeof(UserRes) =>
             new UserRes(src.UserId, src.Username, src.AvatarUrl, src.Email, src.PhoneNumber, src.StudentId, src.CardUrl,
                         src.Verified.ToString(), src.ProcessNote, null, null, src.Role?.RoleName)
             {
                 IsHaveUnreadNoti = isHaveUnreadNoti 
             } as TEntity,

            Type t when t == typeof(GetListUserRes) =>
                new GetListUserRes(src.UserId, src.Username, src.AvatarUrl, src.Email, src.PhoneNumber, src.StudentId, src.CardUrl,
                                   src.Verified, src.Role!.RoleName, src.CreatedAt, src.UpdatedAt, src.IsDeleted,
                                   src.DeletedAt) as TEntity,

            Type t when t == typeof(OrganizerReportInfo) =>
                new OrganizerReportInfo(src.UserId, src.Username, src.AvatarUrl, noOfEvent??0) as TEntity,

            _ => throw new InvalidOperationException($"Unsupported type: {typeof(TEntity).Name}")
        };

        return result ?? throw new InvalidOperationException($"Failed to cast to {typeof(TEntity).Name}");
    }
}
