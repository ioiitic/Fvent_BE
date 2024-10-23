using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;


public static class ReviewMapper
{
    public static EventReview ToReview(
        this CreateReviewReq src, Guid eventId)
        => new(
            src.Rating,
            src.Comment,
            eventId,
            src.UserId,
            DateTime.UtcNow);

    public static ReviewRes ToReponse(
        this EventReview src,
        string fullname)
        => new(
            src.Rating,
            src.Comment,
            fullname);
}
