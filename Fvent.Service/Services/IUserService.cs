using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IUserService
{
    #region User
    Task<AuthResponse> Authen(AuthReq req);
    Task<UserRes> GetByEmail(string email);
    Task<IdRes> Update(Guid id, UpdateUserReq req);
    #endregion

    #region Student
    #endregion

    #region Organizer
    #endregion

    #region Moderater
    #endregion

    #region Admin
    #endregion
    Task<PageResult<GetListUserRes>> GetList(GetListUsersReq req);
    Task<UserRes> Get(Guid id);
    Task<IdRes> Register(CreateUserReq req);
    Task<bool> VerifyEmailAsync(Guid userId, string token);
    Task RequestPasswordResetAsync(string email);
    Task ResetPasswordAsync(Guid userId, string token, string newPassword);
    Task Delete(Guid id);
}
