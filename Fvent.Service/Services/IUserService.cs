using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IUserService
{
    Task<AuthResponse> Authen(AuthReq req);
    Task<IList<GetListUserRes>> GetListUsers(GetListUsersReq req);
    Task<UserRes> GetUser(Guid id);
    Task<IdRes> RegisterUser(CreateUserReq req);
    Task<IdRes> UpdateUser(Guid id, UpdateUserReq req);
    Task DeleteUser(Guid id);
}
