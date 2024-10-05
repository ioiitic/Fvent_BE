using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IUserService
{
    #region User
    /// <summary>
    /// Service for User Login
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<AuthResponse> Authen(AuthReq req);

    /// <summary>
    /// Service for User Get own info
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<UserRes> GetByEmail(string email);

    /// <summary>
    /// Service for User Update info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
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
    Task<IList<GetListUserRes>> GetList(GetListUsersReq req);
    Task<UserRes> Get(Guid id);
    Task<IdRes> Register(CreateUserReq req);
    Task Delete(Guid id);
}
