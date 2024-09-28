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
    public async Task<AuthResponse> Authen(AuthReq req)
    {
        var spec = new AuthenUserSpec(req.Email, req.Password);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        var token = JS.GenerateToken(user.Username, user.Role!, configuration);

        return new AuthResponse(token);
    }

    public async Task<IdRes> RegisterUser(CreateUserReq req)
    {
        var user = req.ToUser();

        await uOW.Users.AddAsync(user);
        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }

    public async Task DeleteUser(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        uOW.Users.Delete(user);

        await uOW.SaveChangesAsync();
    }

    public async Task<IList<UserRes>> GetListUsers()
    {
        var spec = new GetUserSpec();
        var users = await uOW.Users.GetListAsync(spec);

        return users.Select(u => u.ToReponse(u.Role!.RoleName)).ToList();
    }

    public async Task<UserRes> GetUser(Guid id)
    {
        var spec = new GetUserSpec(id);
        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        return user.ToReponse(user.Role!.RoleName);
    }

    public async Task<IdRes> UpdateUser(Guid id, UpdateUserReq req)
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
            req.CardUrl,
            req.Campus,
            req.RoleId);

        if (uOW.IsUpdate(user))
            user.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return user.UserId.ToResponse();
    }
}
