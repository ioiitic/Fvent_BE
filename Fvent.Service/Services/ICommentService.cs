using Fvent.Service.Request;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services
{
    public interface ICommentService
    {
        Task<IdRes> CreateComment(Guid eventId, CreateCommentReq req);
        Task<IList<CommentRes>> GetListComments(Guid eventId);
    }
}
