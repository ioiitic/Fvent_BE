using Fvent.BO.Entities;
using Fvent.Repository.Common;

namespace Fvent.Service.Specifications;

public static class TagSpec
{
    public class GetTagSpec : Specification<Tag>
    {
        public GetTagSpec(Guid TagId)
        {
            Filter(u => u.TagId == TagId);
        }
        public GetTagSpec()
        {
            OrderBy(u => u.TagName, false);
        }
    }
}
