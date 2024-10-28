using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IRatingService
{
    Task<EventRateRes> GetEventRate(Guid req);
}
    