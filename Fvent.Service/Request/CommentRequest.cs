using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Request
{
    public record CreateCommentReq(Guid userId, string commentText);
}
