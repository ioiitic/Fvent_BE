using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fvent.Service.Specifications.EventTypeSpec;
using static Fvent.Service.Specifications.ReviewSpec;

namespace Fvent.Service.Services.Imp
{
    public class EventTypeService(IUnitOfWork uOW) : IEventTypeService
    {
        public async Task<IdRes> CreateEventType(string eventTypeName)
        {
            var newType = new EventType(eventTypeName);

            await uOW.EventType.AddAsync(newType);
            await uOW.SaveChangesAsync();

            return newType.EventTypeId.ToResponse();
        }

        public async Task DeleteEventType(Guid id)
        {
            var spec = new GetEventTypeSpec(id);
            var eventType = await uOW.EventType.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(EventReview));

            uOW.EventType.Delete(eventType);

            await uOW.SaveChangesAsync();
        }

        public async Task<IList<EventType>> GetListEventTypes()
        {
            var spec = new GetEventTypeSpec();
            var eventTypes = await uOW.EventType.GetListAsync(spec);

            return eventTypes.ToList();
        }

        public async Task<EventType> GetEventType(Guid id)
        {
            var spec = new GetEventTypeSpec(id);
            var eventType = await uOW.EventType.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(EventType));

            return eventType;
        }

        public async Task<IdRes> UpdateEventType(Guid id, string newName)
        {
            var spec = new GetEventTypeSpec(id);
            var eventType = await uOW.EventType.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(EventType));

            eventType.Update(newName);

            await uOW.SaveChangesAsync();

            return eventType.EventTypeId.ToResponse();
        }
    }
}
