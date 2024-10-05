using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class UserSpec
{
    public class GetListUsersSpec : Specification<User>
    {
        public GetListUsersSpec(string? username, string? email, string? roleName, bool? verified)
        {
            if (!string.IsNullOrEmpty(username))
            {
                Filter(u => u.Username.Contains(username));
            }
            if (!string.IsNullOrEmpty(email))
            {
                Filter(u => u.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                Filter(u => u.Role!.RoleName.Contains(roleName));
            }
            if (verified is not null)
            {
                Filter(u => u.Verified == verified);
            }

            Include(u => u.Role!);
        }
    }

    public class GetUserSpec : Specification<User>
    {
        public GetUserSpec()
        {
            Include(u => u.Role!);
        }

        public GetUserSpec(Guid id)
        {
            Filter(u => u.UserId == id);

            Include(u => u.Role!);
        }

        public GetUserSpec(string email)
        {
            Filter(u => u.Email == email);

            Include(u => u.Role!);
        }
    }

    public class AuthenUserSpec : Specification<User>
    {
        public AuthenUserSpec(string Email, string Password)
        {
            Filter(u => u.Email.Equals(Email) && u.Password.Equals(Password));

            Include(u => u.Role!);
        }
    }
}
