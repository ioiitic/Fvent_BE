using Fvent.BO.Common;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IUserService
{
    #region Auth
    Task<AuthRes> Authen(AuthReq req, string ipAddress);
    Task<AuthRes> Refresh(RefreshTokenReq req, string ipAddress);
    #endregion

    #region User
    Task<PageResult<GetListUserRes>> GetList(GetListUsersReq req);
    Task<PageResult<GetListUserRes>> GetListBannedUser(GetListUsersReq req);
    Task<UserRes> Get(Guid id);
    Task<IdRes> Update(Guid id, UpdateUserReq req);
    Task Delete(Guid id);
    Task UnBan(Guid id);
    #endregion

    #region User Account
    Task<IdRes> Register(CreateUserReq req);
    Task<IdRes> AddCardId(Guid id, string cardUrl);
    Task<IdRes> ApproveUser(Guid id, bool isApproved, string processNote);
    Task<IdRes> RegisterModerator(CreateModeratReq req);
    #endregion

    #region Email
    Task VerifyEmailAsync(Guid userId, string token);
    Task RequestPasswordResetAsync(string email);
    Task ResetPasswordAsync(Guid userId, string token, string newPassword);
    Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    Task<IdRes> ResendVerificationEmail(string userEmail, string role);
    #endregion

    #region Report
    Task<UserReportRes> GetReport(Guid userId);
    #endregion
}
