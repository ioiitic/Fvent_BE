using Fvent.BO.Entities;
using Fvent.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Specifications
{
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
            }
        }
    }
}
