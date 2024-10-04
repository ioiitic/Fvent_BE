using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fvent.Service.Specifications.EventFollowerSpec;

namespace Fvent.Service.Services.Imp
{
    public class EventFollowerService(IUnitOfWork uOW) : IEventFollowerService
    {
        /// <summary>
        /// Add event to user Eventfollow
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IdRes> FollowEvent(Guid eventId, Guid userId)
        {
            EventFollower _eventFollower = new EventFollower(eventId, userId);

            await uOW.EventFollower.AddAsync(_eventFollower);
            await uOW.SaveChangesAsync();

            return _eventFollower.EventId.ToResponse();
        }

        /// <summary>
        /// Unfollow an event by using eventId and userId
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>

        public async Task UnfollowEvent(Guid eventId, Guid userId)
        {
            var spec = new GetEventFollowerSpec(eventId, userId);
            var _event = await uOW.EventFollower.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(Event));

            uOW.EventFollower.Delete(_event);

            await uOW.SaveChangesAsync();
        }

        public async Task<IList<EventRes>> GetFollowedEvents(Guid userId)
        {
            // Specification to get followed events for a user
            var spec = new GetEventFollowerSpec(userId);

            // Get the list of followed events
            var followedEvents = await uOW.EventFollower.GetListAsync(spec);

            // Map followed events to EventRes
            var followedEventResponses = followedEvents
                .Select(f => f.Event!.ToResponse(
                    f.Event.Organizer!.FirstName + " " + f.Event.Organizer.LastName,
                    f.Event.EventType!.EventTypeName,
                    null))
                .ToList();

            return followedEventResponses;
        }
    }
}
