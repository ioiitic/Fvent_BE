using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Mapper
{
    public static class CommentMapper
    {
        public static Comment ToComment(
            this CreateCommentReq src,
            Guid eventId)
            => new(
                eventId,
                src.userId,
                src.commentText,
                DateTime.UtcNow);

        public static CommentRes ToReponse(
            this Comment src)
            => new(
                src.CommentId,
                src.EventId,
                src.UserId,
                src.CommentText,
                src.CreatedAt);
    }
}
