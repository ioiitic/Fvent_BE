using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IReviewService
{
    Task<IList<ReviewRes>> GetListReviews();
    Task<ReviewRes> GetReview(Guid id);
    Task<IdRes> CreateReview(CreateReviewReq req);
    Task<IdRes> UpdateReview(Guid id, UpdateReviewReq req);
    Task DeleteReview(Guid id);
}
