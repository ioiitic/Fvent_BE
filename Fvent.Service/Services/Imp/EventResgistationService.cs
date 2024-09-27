using Fvent.BO.Entities;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services.Imp
{
    public class EventResgistationService(IUnitOfWork uOW) : IEventResgistationService
    {
        /// <summary>
        /// Register Free Event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IdRes> RegisterFreeEvent(Guid eventId, Guid userId)
        {
            EventRegistration _eventFollower = new EventRegistration(eventId, userId);

            await uOW.EventRegistration.AddAsync(_eventFollower);
            await uOW.SaveChangesAsync();

            return _eventFollower.EventId.ToResponse();
        }
    }
}
