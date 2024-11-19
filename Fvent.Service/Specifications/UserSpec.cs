using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class UserSpec
{
    public class GetListUsersSpec : Specification<User>
    {
        public GetListUsersSpec(string? username, string? email, string? roleName, string? verified, string? orderBy,
                                bool isDescending, int pageNumber, int pageSize)
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

            // Filter by verified status if provided
            if (!string.IsNullOrEmpty(verified) && Enum.TryParse<VerifiedStatus>(verified, true, out var verifiedStatus))
            {
                Filter(e => e.Verified == verifiedStatus);
            }

            if (orderBy is not null)
            {
                switch (orderBy)
                {
                    case "email":
                        OrderBy(u => u.Email, isDescending);
                        break;
                }
            }
            AddPagination(pageNumber, pageSize);

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
            Include(u => u.RefreshTokens!);
        }
    }
    public class GetVerificationTokenSpec : Specification<VerificationToken>
    {
        public GetVerificationTokenSpec(Guid userId, string token)
        {
            Filter(u => u.UserId == userId && u.Token.Equals(token));
        }

        public GetVerificationTokenSpec(Guid userId)
        {
            Filter(u => u.UserId == userId);
        }
    }

    public class CheckRefreshTokenSpec : Specification<RefreshToken>
    {
        public CheckRefreshTokenSpec(string token)
        {
            Filter(t => t.Token == token);
        }
    }
  
    public class GetRefreshTokenSpec : Specification<RefreshToken>
    {
        GetRefreshTokenSpec(string token)
        {
            Filter(t => t.Token == token);

            Include("User.Role");
        }
    }
}
