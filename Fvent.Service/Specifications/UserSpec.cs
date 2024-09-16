using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class UserSpec
{
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
    }
}
