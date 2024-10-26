using Fvent.BO.Entities;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services
{
    public interface IEventTypeService
    {
        Task<IdRes> CreateEventType(string eventTypeName);
        Task DeleteEventType(Guid id);
        Task<IList<EventType>> GetListEventTypes();
        Task<EventType> GetEventType(Guid id);
        Task<IdRes> UpdateEventType(Guid id, string newName);
    }
}
