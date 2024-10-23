using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services
{
    public interface IFollowerService
    {
        Task<IdRes> FollowEvent(Guid id, Guid userId);
        Task UnfollowEvent(Guid eventId, Guid userId);
        Task<IList<EventRes>> GetFollowedEvents(Guid userId);
    }
}
