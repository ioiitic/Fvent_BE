using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Microsoft.IdentityModel.Tokens;
using static Fvent.Service.Specifications.EventSpec;

namespace Fvent.Service.Services.Imp;

public class RatingService(IUnitOfWork uOW) : IRatingService
{
    public async Task<EventRateRes> GetEventRate(Guid req)
    {
        var spec = new GetEventRateSpec(req);
        var reviews = await uOW.Reviews.GetListAsync(spec);

        double res = 0;

        if (!reviews.IsNullOrEmpty())
        {
            res = reviews.Sum(r => r.Rating) * 1.0 / reviews.Count();
        }

        int total = reviews.Count();

        return res.ToResponse(total);
    }
}
