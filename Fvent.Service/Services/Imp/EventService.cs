using Azure;
using FirebaseAdmin.Messaging;
using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq.Expressions;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.EventSpec;
using static Fvent.Service.Specifications.EventTagSpec;
using static Fvent.Service.Specifications.ReviewSpec;
using static Fvent.Service.Specifications.UserSpec;

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

        if(req.Proposal is not null)
        {
            EventFile eventFile = new(req.Proposal, _event.EventId);
            await uOW.EventFile.AddAsync(eventFile);
        }
       
        await uOW.EventMedia.AddAsync(poster);
        await uOW.EventMedia.AddAsync(thumbnail); 

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task<IdRes> UpdateEvent(Guid id, Guid organizerId, UpdateEventReq req)
    {
        var serviceKeyPath = Path.Combine(AppContext.BaseDirectory, "firebase-service-key.json");
        var firebaseService = new FirebaseService(serviceKeyPath);
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        if (req.CreateFormDetailsReq is not null)
        {
            var formDetails = req.CreateFormDetailsReq.Select(f => new FormDetail(f.name, f.type, f.options));
            var form = new Form
            {
                FormDetails = formDetails.ToList(),
            };

            _event.Form = form;
        }

        await uOW.SaveChangesAsync();
        if (req.EventTags is not null)
        {
            _event.Tags = _event.Tags ?? [];
            _event.Tags.ForEach(tag => uOW.EventTag.Delete(tag));
            foreach (var item in req.EventTags)
            {
                EventTag tag = new EventTag(_event.EventId, (string)item);
                _event.Tags.Add(tag);
            }
        }

        await uOW.SaveChangesAsync();
        if (req.PosterImg is not null)
        {
            EventMedia poster = new(_event.EventId, (int)MediaType.Poster, req.PosterImg);

            _event.EventMedias = _event.EventMedias ?? [];
            _event.EventMedias.ForEach(media => uOW.EventMedia.Delete(media));
            _event.EventMedias.Add(poster);
        }
        await uOW.SaveChangesAsync();
        if (req.ThumbnailImg is not null)
        {
            EventMedia thumbnail = new(_event.EventId, (int)MediaType.Thumbnail, req.ThumbnailImg);

            _event.EventMedias = _event.EventMedias ?? [];
            _event.EventMedias.ForEach(media => uOW.EventMedia.Delete(media));
            _event.EventMedias.Add(thumbnail);
        }
        await uOW.SaveChangesAsync();
        if (req.Proposal is not null)
        {
            EventFile eventFile = new(req.Proposal, _event.EventId);

            _event.EventFile = eventFile;
        }

        await uOW.SaveChangesAsync();
        _event.Update(req.EventName, req.Description, req.StartTime, req.EndTime, req.Location, req.LinkEvent,
                      req.PasswordMeeting, req.MaxAttendees, req.EventTypeId);

        await uOW.SaveChangesAsync();
        if (uOW.IsUpdate(_event))
        {
            _event.UpdatedAt = DateTime.UtcNow;
        }

        await uOW.SaveChangesAsync();

        // Step 4: Notify users (track unique users with HashSet)
        var notifiedUsers = new HashSet<Guid>();

        // Step 6: Notify users who have registered for the event
        var participantSpec = new GetEventParticipantsSpec(id);

        var participants = await uOW.EventRegistration.GetListAsync(participantSpec);
        var registeredUsers = participants.Select(p => p.UserId).ToList();
        var fcmTokens = participants.Select(p => p.User!.FcmToken).ToList();

        foreach (var userId in registeredUsers)
        {
            if (notifiedUsers.Add(userId)) // Only notify if userId is not already in the set
            {
                // Create notification for the participant
                var notificationReq = new CreateNotificationReq(userId,
                                                _event.EventId,
                                                "Đã có một sự thay đổi bất ngờ!!!",
                                                $"Sự kiện '{_event.EventName}' bạn đăng kí tham gia có cập nhật mới.");

                await CreateNotification(notificationReq);
            }
        }



        // Send a single notification to the user
        await firebaseService.SendBulkNotificationsAsync(fcmTokens,
                                                        "Đã có một sự thay đổi bất ngờ!!!",
                                                        $"Sự kiện '{_event.EventName}' bạn đăng kí tham gia có cập nhật mới."
        );

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
        var spec = new GetEventSpec(req.SearchKeyword, req.InMonth, req.InYear, req.EventTypes, req.EventTag,req.Status, 
                                    req.OrderBy, req.IsDescending, req.PageNumber, req.PageSize);

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

    public async Task<PageResult<EventRes>> GetListEventsForAdmin(GetEventsRequest req)
    {
        var spec = new GetEventAdminSpec(req.SearchKeyword, req.InMonth, req.InYear, req.EventTypes, req.EventTag, req.Status, req.OrderBy, req.IsDescending, req.PageNumber, req.PageSize);

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

    public async Task<List<EventBannerRes>> GetEventBanners()
    {
        var spec = new GetEventSpec();
        var events = await uOW.Events.GetListAsync(spec);

        var eventBanners = events.Take(6).Select(t => t.ToBannerResponse()).ToList();

        return eventBanners;
        
    }

    /// <summary>
    /// Get Event Detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<EventRes> GetEvent(Guid eventId, Guid? userId)
    {
        // Flags for user-specific event details
        bool isRegistered = false;
        bool isReviewed = false;
        bool isOverlap = false;
        bool canReview = false;

        // Fetch the event details
        var specEvent = new GetEventSpec(eventId);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(specEvent)
            ?? throw new NotFoundException(typeof(Event));

        if (userId.HasValue)
        {
            // User-specific queries
            var userIdValue = userId.Value;

            var specRegis = new GetEventRegistrationSpec(eventId, userIdValue);
            var specReview = new GetReviewSpec(eventId, userId);
            var specOverlap = new GetEventRegistrationSpec(userIdValue, _event.EventId, _event.StartTime, _event.EndTime);

            // Run queries concurrently
            var taskEventReview = uOW.Reviews.GetListAsync(specReview);
            var taskEventOverlap = uOW.EventRegistration.GetListAsync(specOverlap);
            var taskEventRegis = uOW.EventRegistration.FindFirstOrDefaultAsync(specRegis);

            await Task.WhenAll(taskEventReview, taskEventOverlap, taskEventRegis);

            var eventReview = taskEventReview.Result;
            var eventOverlap = taskEventOverlap.Result;
            var eventRegis = taskEventRegis.Result;

            // Set flags based on results
            isReviewed = !eventReview.IsNullOrEmpty();
            isOverlap = !eventOverlap.IsNullOrEmpty();
            isRegistered = eventRegis is not null;

            // Logic to determine if the user can review
            if (_event.EndTime <= DateTime.Now && _event.EndTime >= DateTime.Now.AddDays(-2) &&
                eventRegis?.IsCheckIn == true)
            {
                canReview = true;
            }
        }

        // Return the event response
        return _event.ToResponse(isRegistered, isReviewed, isOverlap, canReview);
    }

    public async Task<IdRes> SubmitEvent(Guid id, Guid organizerId)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        if (_event.OrganizerId != organizerId)
            throw new Exception("Not have permission for submit Event");

        if(_event.Status == EventStatus.Draft || _event.Status == EventStatus.Rejected)
            _event.Status = EventStatus.UnderReview;
        
        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task<IdRes> ApproveEvent(Guid id, bool isApproved,Guid userId, string processNote)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        var specSub = new GetUserSpec(userId);
        var _moderator = await uOW.Users.FindFirstOrDefaultAsync(specSub);
        
        if(_event.Status == EventStatus.UnderReview)
        {
            if (isApproved)
            {
                _event.Status = EventStatus.Upcoming;
            }
            else
            {
                _event.Status = EventStatus.Rejected;
            }
            _event.ProcessNote = processNote;
            _event.ReviewBy = _moderator!.Username;
        }

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task CheckinEvent(Guid eventId, Guid userId, bool isOrganzier)
    {
        var spec = new GetEventRegistrationSpec(eventId, userId);
        var _event = await uOW.EventRegistration.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(EventRegistration));

        var specSub = new GetEventSpec(eventId);
        var checkEvent = await uOW.Events.FindFirstOrDefaultAsync(specSub)
            ?? throw new NotFoundException(typeof(Event));
        if(checkEvent.Status != EventStatus.InProgress)
        {
            throw new Exception("Sự kiện đang chưa đến hạn/quá hạn checkin");
        }
        if (isOrganzier)
        {
            if(_event.IsCheckIn == true)
            {
                _event.IsCheckIn = false;
            }
            else _event.IsCheckIn = true;
        }
        else
        {
            _event.IsCheckIn = true;
        }
        await uOW.SaveChangesAsync();
    }


    public async Task<IList<EventRes>> GetListEventsByOrganizer(GetEventByOrganizerReq req)
    {
        var spec = new GetEventByOrganizerSpec(req.OrganizerId, req.Status);

        var _events = await uOW.Events.GetListAsync(spec);

        return _events.Select(e => e.ToResponse()).ToList();
    }

    /// <summary>
    /// Only organizer can see all event belong to them
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<PageResult<EventRes>> GetListEventsOfOrganizer(GetEventOfOrganizerReq req)
    {
        var spec = new GetEventByOrganizerSpec(req.userId, req.SearchKeyword, req.InMonth, req.InYear, req.EventTypes, req.EventTag, req.Status,
                                               req.PageNumber, req.PageSize);

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
    #endregion

    #region Event-User
    public async Task<IList<EventRes>> GetRegisteredEvents(Guid userId,int? inMonth,int? inYear, bool isCompleted)
    {
        var spec = new GetRegisteredEventsSpec(userId, inMonth, inYear, isCompleted);
        var events = await uOW.Events.GetListAsync(spec);

        return events.Select(e => e.ToResponse()).ToList();
    }


    public async Task<PageResult<UserRes>> GetRegisteredUsers(Guid eventId, GetRegisteredUsersReq req, Guid userId)
    {
        // Create the specification
        var spec = new GetRegisteredUsersSpec(eventId);

        // Check if the event belongs to the organizer
        var checkEvent = await uOW.Events.FindFirstOrDefaultAsync(new GetEventSpec(eventId));
        if (checkEvent != null && checkEvent.OrganizerId != userId)
        {
            throw new Exception("This event does not belong to this organizer.");
        }

        // Get paginated events with registrations
        var events = await uOW.Events.GetListAsync(spec);

        // Extract all users from registrations
        var allUsers = events.SelectMany(e => e.Registrations!)

                             .Select(r =>
                             {
                                 var userResponse = r.User!.ToResponse<UserRes>();
                                 return userResponse with { IsCheckin = r.IsCheckIn };
                             })
                             .ToList();

        if (!string.IsNullOrEmpty(req.SearchKeyword))
        {
            var keyword = req.SearchKeyword.ToLower(); 
            allUsers = allUsers
                .Where(u => u.Username!.ToLower().Contains(keyword) ||
                            u.Email!.ToLower().Contains(keyword))
                .ToList();  
        }



        // Apply paging on the flattened user list
        var paginatedUsers = allUsers.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();

        // Create PageResult for the users
        return new PageResult<UserRes>(
            paginatedUsers,
            req.PageNumber,
            req.PageSize,
            allUsers.Count, // Total number of users
            allUsers.Count, // Total items is the total count of all users
            (int)Math.Ceiling(allUsers.Count / (double)req.PageSize) // Calculate total pages
        );

    }

    #endregion

    public async Task CreateNotification(CreateNotificationReq req)
    {
        var notification = req.ToNotification();

        await uOW.Notification.AddAsync(notification);
        await uOW.SaveChangesAsync();
    }
}
