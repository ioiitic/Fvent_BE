using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IReviewService
{
    Task<IList<ReviewRes>> GetListReviews(Guid eventId);
    Task<ReviewRes> GetReview(Guid id);
    Task<IdRes> CreateReview(Guid eventId, Guid userId, CreateReviewReq req);
    Task<IdRes> UpdateReview(Guid id, Guid userId, UpdateReviewReq req);
    Task DeleteReview(Guid id);
}
