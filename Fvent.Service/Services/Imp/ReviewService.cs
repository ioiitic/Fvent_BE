using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using static Fvent.Service.Specifications.ReviewSpec;

namespace Fvent.Service.Services.Imp;

public class ReviewService(IUnitOfWork uOW) : IReviewService
{
    public async Task<IdRes> CreateReview(Guid eventId, CreateReviewReq req)
    {
        var review = req.ToReview(eventId);

        await uOW.Reviews.AddAsync(review);
        await uOW.SaveChangesAsync();

        return review.EventId.ToResponse();
    }

    public async Task DeleteReview(Guid id)
    {
        var spec = new GetReviewSpec(id);
        var review = await uOW.Reviews.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(EventReview));

        uOW.Reviews.Delete(review);

        await uOW.SaveChangesAsync();
    }

    public async Task<IList<ReviewRes>> GetListReviews()
    {
        var spec = new GetReviewSpec();
        var reviews = await uOW.Reviews.GetListAsync(spec);

        return reviews.Select(r => r.ToReponse(r.User!.Username)).ToList();
    }

    public async Task<ReviewRes> GetReview(Guid id)
    {
        var spec = new GetReviewSpec(id);
        var review = await uOW.Reviews.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(EventReview));

        return review.ToReponse(review.User!.Username);
    }

    public async Task<IdRes> UpdateReview(Guid id, UpdateReviewReq req)
    {
        var spec = new GetReviewSpec(id);
        var review = await uOW.Reviews.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(EventReview));

        review.Update(req.Rating,
            req.Comment,
            review.EventId,
            req.UserId);

        if (uOW.IsUpdate(review))
            review.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return review.EventId.ToResponse();
    }
}
