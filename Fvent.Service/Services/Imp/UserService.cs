using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Microsoft.Extensions.Configuration;
using static Fvent.Service.Specifications.UserSpec;
using JS = Fvent.Service.Utils.JwtService;

namespace Fvent.Service.Services.Imp;

public class UserService(IUnitOfWork uOW, IConfiguration configuration) : IUserService
{
    #region User
    /// <summary>
    /// Implement service for User Login
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<AuthResponse> Authen(AuthReq req)
    {
        var spec = new AuthenUserSpec(req.Email, req.Password);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        var token = JS.GenerateToken(user.Email, user.Role!, configuration);

        return new AuthResponse(token);
    }

    /// <summary>
    /// Implement service for User Get own info
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserRes> GetByEmail(string email)
    {
        var spec = new GetUserSpec(email);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user.ToResponse<UserRes>();
    }

    /// <summary>
    /// Implement service for User Update info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<IdRes> Update(Guid id, UpdateUserReq req)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        user.Update(req.Username,
            req.AvatarUrl,
            req.Email,
            req.Password,
            req.FirstName,
            req.LastName,
            req.PhoneNumber,
            req.CardUrl);

        if (uOW.IsUpdate(user))
            user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }
    #endregion

    #region Admin
    /// <summary>
    /// Service for Admin Get list users info
    /// </summary>
    /// <returns></returns>
    public async Task<IList<GetListUserRes>> GetList(GetListUsersReq req)
    {
        var spec = new GetListUsersSpec(req.Username, req.Email, req.RoleName, req.Verified);
        var users = await uOW.Users.GetListAsync(spec);

        return users.Select(u => u.ToResponse<GetListUserRes>()).ToList();
    }
    #endregion

    public async Task<IdRes> Register(CreateUserReq req)
    {
        var user = req.ToUser();

        await uOW.Users.AddAsync(user);
        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }

    public async Task Delete(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        uOW.Users.Delete(user);

        await uOW.SaveChangesAsync();
    }

    public async Task<UserRes> Get(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user.ToResponse<UserRes>();
    }
}
