using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IUserService
{
    #region Authen
    Task<AuthRes> Authen(AuthReq req, string ipAddress);
    Task<AuthRes> Refresh(RefreshTokenReq req, string ipAddress);
    Task<IdRes> Register(CreateUserReq req);
    #endregion

    #region User
    Task<PageResult<GetListUserRes>> GetList(GetListUsersReq req);
    Task<UserRes> Get(Guid id);
    Task<UserRes> GetByEmail(string email);
    Task<IdRes> Update(Guid id, UpdateUserReq req);
    #endregion

    #region Verify
    Task<IdRes> AddCardId(Guid id, string cardUrl);
    Task<IdRes> ApproveUser(Guid id, bool isApproved, string processNote);
    #endregion

    #region Email
    Task VerifyEmailAsync(Guid userId, string token);
    Task RequestPasswordResetAsync(string email);
    #endregion

    Task ResetPasswordAsync(Guid userId, string token, string newPassword);
    Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    Task Delete(Guid id);
    #region Report
    Task<UserReportRes> GetReport(Guid userId);
    #endregion
}
