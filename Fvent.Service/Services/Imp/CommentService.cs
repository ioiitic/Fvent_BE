using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Fvent.Service.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fvent.Service.Specifications.ReviewSpec;

namespace Fvent.Service.Services.Imp
{
    public class CommentService(IUnitOfWork uOW) : ICommentService
    {
        public async Task<IdRes> CreateComment(Guid eventId, CreateCommentReq req)
        {
            var _comment = req.ToComment(eventId);

            await uOW.Comment.AddAsync(_comment);
            await uOW.SaveChangesAsync();

            return _comment.CommentId.ToResponse();
        }

        public async Task<IList<CommentRes>> GetListComments(Guid eventId)
        {
            var spec = new GetCommentSpec(eventId);

            var comments = await uOW.Comment.GetListAsync(spec);

            return comments.Select(r => r.ToReponse()).ToList();
        }
    }
}
