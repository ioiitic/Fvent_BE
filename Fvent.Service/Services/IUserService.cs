using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IUserService
{
    Task<AuthResponse> Authen(AuthReq req);
    Task<IList<UserRes>> GetListUsers();
    Task<UserRes> GetUser(Guid id);
    Task<IdRes> CreateUser(CreateUserReq req);
    Task<IdRes> UpdateUser(Guid id, UpdateUserReq req);
    Task DeleteUser(Guid id);
}
