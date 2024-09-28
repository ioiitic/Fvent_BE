using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Result
{
    public record CommentRes(Guid commentId,
                             Guid eventId,
                             Guid userId,
                             string commentText,
                             DateTime createAt);
}
