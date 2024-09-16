using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;


public static class ReviewMapper
{
    public static EventReview ToReview(
        this CreateReviewReq src)
        => new(
            src.Rating,
            src.Comment,
            src.EventId,
            src.UserId,
            DateTime.UtcNow);

    public static ReviewRes ToReponse(
        this EventReview src,
        string eventName,
        string fullname)
        => new(
            src.Rating,
            src.Comment,
            eventName,
            fullname);
}
