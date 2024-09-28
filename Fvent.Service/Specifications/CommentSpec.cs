using Fvent.BO.Entities;
using Fvent.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Specifications
{
    public class GetCommentSpec : Specification<Comment>
    {
        public GetCommentSpec()
        {
            Include(u => u.User!);
            Include(u => u.Event!);
        }
        public GetCommentSpec(Guid id)
        {
            Filter(u => u.EventId == id);

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }
}
