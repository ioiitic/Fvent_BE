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
using static Fvent.Service.Specifications.EventRegistationSpec;

namespace Fvent.Service.Services.Imp
{
    public class EventRegistationService(IUnitOfWork uOW) : IEventRegistationService
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

        public async Task<IList<UserRes>> GetAllParticipantsForEvent(Guid eventId)
        {
            // Define a specification to get participants for the event
            var spec = new GetEventParticipantsSpec(eventId);

            // Get the list of participants for the event
            var participants = await uOW.EventRegistration.GetListAsync(spec);

            // Map participants to UserRes (or any suitable response model)
            var participantResponses = participants
                .Select(p => p.User!.ToResponse<UserRes>())
                .ToList();

            return participantResponses;
        }

    }
}
