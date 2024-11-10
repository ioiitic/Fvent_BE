using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;


public static class ReviewMapper
{
    public static EventReview ToReview(
        this CreateReviewReq src, Guid eventId, Guid userId)
        => new(
            src.Rating,
            src.Comment,
            eventId,
            userId,
            DateTime.Now);

    public static ReviewRes ToReponse(
        this EventReview src,
        User user)
        => new(
            src.Rating,
            src.Comment,
            user.Username,
            user.AvatarUrl);
}
