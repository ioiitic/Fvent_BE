using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Fvent.Service.Specifications;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.EventSpec;
using static Fvent.Service.Specifications.EventTagSpec;

namespace Fvent.Service.Services.Imp;

public class EventService(IUnitOfWork uOW) : IEventService
{
    #region Student
    public async Task<PageResult<EventRes>> GetListRecommend(Guid userId)
    {
        var registerSpec = new GetListUserEventsSpec(userId);
        var userEventRegister = await uOW.EventRegistration.GetListAsync(registerSpec);
        var userEvents = userEventRegister.Select(r => r.Event);
        var eventTypes = userEvents.Select(e => e.EventTypeId).Distinct();
        var eventTags = userEvents.SelectMany(e => e.Tags!.Select(t => t.Tag)).Distinct();

        var eventSpec = new GetListRecommend(eventTypes, eventTags);

        var events = await uOW.Events.GetListAsync(eventSpec);

        var res = events.Select(e => e.ToResponse()).ToList();

        return new PageResult<EventRes>(res, 1, 1, 1, 1, 1);
    }
    #endregion

    #region CRUD Event
    public async Task<IdRes> CreateEvent(CreateEventReq req, Guid organizerId)
    {
        var _event = req.ToEvent(organizerId);
        
        if(req.CreateFormDetailsReq is not null)
        {
            var formDetails = req.CreateFormDetailsReq.Select(f => new FormDetail(f.name, f.type, f.options));
            var form = new Form
            {
                FormDetails = formDetails.ToList(),
            };

            _event.Form = form;
           
            
        }
        await uOW.Events.AddAsync(_event);
        await uOW.SaveChangesAsync();
        foreach (var item in req.EventTags)
        {
            EventTag tag = new EventTag(_event.EventId, (string)item);
            await uOW.EventTag.AddAsync(tag);
        }
        await uOW.SaveChangesAsync();

        EventMedia poster = new(_event.EventId, (int)MediaType.Poster, req.PosterImg);
        EventMedia thumbnail = new(_event.EventId, (int)MediaType.Thumbnail, req.ThumbnailImg);

        if(req.proposal is not null)
        {
            EventFile eventFile = new(req.proposal, _event.EventId);
            await uOW.EventFile.AddAsync(eventFile);
        }
       
        await uOW.EventMedia.AddAsync(poster);
        await uOW.EventMedia.AddAsync(thumbnail); 

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task DeleteEvent(Guid id)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        uOW.Events.Delete(_event);

        await uOW.SaveChangesAsync();
    }

    public async Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req)
    {
        var spec = new GetEventSpec(req.SearchKeyword, req.InMonth, req.InYear, req.EventTypes, req.EventTag,req.Status, req.OrderBy, req.IsDescending, req.PageNumber, req.PageSize);

        // Get paginated list of events
        var _events = await uOW.Events.GetPageAsync(spec);

        // Map each event to EventRes with the ToResponse extension method
        var eventResponses = _events.Items.Select(eventEntity => eventEntity.ToResponse()).ToList();

        return new PageResult<EventRes>(
            eventResponses,
            _events.PageNumber,
            _events.PageSize,
            _events.Count,
            _events.TotalItems,
            _events.TotalPages
        );
    }



    /// <summary>
    /// Get Event Detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<EventRes> GetEvent(Guid eventId, Guid? userId)
    {
        bool isRegistered = false;
        var spec = new GetEventSpec(eventId);

        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        if (userId.HasValue)
        {
            var subSpec = new GetEventRegistrationSpec(eventId, userId.Value);
            var _eventTag = await uOW.EventRegistration.GetListAsync(subSpec);

            if (!_eventTag.IsNullOrEmpty())
            {
                isRegistered = true;
            }
        }

        return _event.ToResponse(isRegistered);
    }


    /// <summary>
    /// Update an Available Event
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<IdRes> UpdateEvent(Guid id, Guid organizerId, UpdateEventReq req)
    {
        // Step 1: Find the event
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        // Step 2: Update the event with the new details
        _event.Update(req.EventName,
            req.Description,
            req.StartTime,
            req.EndTime,
            req.Location,
            req.MaxAttendees,
            req.ProcessNote,
            req.Status,
            organizerId,
            req.EventTypeId);

        if (uOW.IsUpdate(_event))
        {
            _event.UpdatedAt = DateTime.UtcNow;
        }

        // Step 3: Save changes to the event
        await uOW.SaveChangesAsync();

        // Step 4: Notify users (track unique users with HashSet)
        var notifiedUsers = new HashSet<Guid>();

        // Step 5: Notify users who follow the event
        var followSpec = new GetUserFollowsEventSpec(id); 

        var followedEvents = await uOW.EventFollower.GetListAsync(followSpec);
        var followedUsers = followedEvents.Select(f => f.UserId).ToList();

        foreach (var userId in followedUsers)
        {
            if (notifiedUsers.Add(userId)) // Only notify if userId is not already in the set
            {
                // Create notification for the follower
                var notificationReq = new CreateNotificationReq(userId,
                                                _event.EventId,
                                                $"The event '{_event.EventName}' has been updated.");
                await CreateNotification(notificationReq); 
            }
        }

        // Step 6: Notify users who have registered for the event
        var participantSpec = new GetEventParticipantsSpec(id);

        var participants = await uOW.EventRegistration.GetListAsync(participantSpec);
        var registeredUsers = participants.Select(p => p.UserId).ToList();

        foreach (var userId in registeredUsers)
        {
            if (notifiedUsers.Add(userId)) // Only notify if userId is not already in the set
            {
                // Create notification for the participant
                var notificationReq = new CreateNotificationReq(userId,
                                                _event.EventId,
                                                $"The event '{_event.EventName}' has been updated.");
                await CreateNotification(notificationReq); 
            }
        }

        return _event.EventId.ToResponse();
    }

    public async Task<IdRes> SubmitEvent(Guid id)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));
        _event.Status = EventStatus.UnderReview;
        
        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task<IdRes> ApproveEvent(Guid id, bool isApproved, string processNote)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));
        if (isApproved)
        {
            _event.Status = EventStatus.Upcoming;
        }
        else
        {
            _event.Status = EventStatus.Rejected;
        }

        _event.ProcessNote = processNote;   

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task CheckinEvent(Guid eventId, Guid userId)
    {
        var spec = new GetEventRegistrationSpec(eventId, userId);
        var _event = await uOW.EventRegistration.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(EventRegistration));
        _event.IsCheckIn = true;

        await uOW.SaveChangesAsync();
    }


    public async Task<IList<EventRes>> GetListEventsByOrganizer(GetEventByOrganizerReq req)
    {
        var spec = new GetEventByOrganizerSpec(req.OrganizerId, req.Status);

        var _events = await uOW.Events.GetListAsync(spec);

        return _events.Select(e => e.ToResponse()).ToList();
    }
    #endregion

    #region Event-User
    public async Task<IList<EventRes>> GetRegisteredEvents(Guid userId)
    {
        var spec = new GetRegisteredEventsSpec(userId);
        var events = await uOW.Events.GetListAsync(spec);

        return events.Select(e => e.ToResponse()).ToList();
    }

    public async Task<IList<UserRes>> GetRegisteredUsers(Guid eventId)
    {
        var spec = new GetRegisteredUsersSpec(eventId);
        var events = await uOW.Events.GetListAsync(spec);

        var users = events.SelectMany(e => e.Registrations!)
            .Select(r => r.User);

        return users.Select(u => u.ToResponse<UserRes>()).ToList();
    }
    #endregion

    public async Task CreateNotification(CreateNotificationReq req)
    {
        var notification = req.ToNotification();

        await uOW.Notification.AddAsync(notification);
        await uOW.SaveChangesAsync();
    }
}
