using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services
{
    public interface IEventRegistationService
    {
        Task<IdRes> RegisterFreeEvent(Guid id, Guid userId);
        Task UnRegisterEvent(Guid eventId, Guid userId);
        Task<IList<UserRes>> GetAllParticipantsForEvent(Guid eventId);
    }
}
