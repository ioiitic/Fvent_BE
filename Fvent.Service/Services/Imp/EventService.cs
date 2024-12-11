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
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.EventSpec;
using static Fvent.Service.Specifications.EventTagSpec;
using static Fvent.Service.Specifications.FormSpec;
using static Fvent.Service.Specifications.ReviewSpec;
using static Fvent.Service.Specifications.UserSpec;

namespace Fvent.Service.Services.Imp;

public class EventService(IUnitOfWork uOW, IEmailService emailService) : IEventService
{
    #region Event
    public async Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req)
    {
        var spec = new GetEventSpec(req.SearchKeyword, req.InMonth, req.InYear, req.EventTypes, req.EventTag, req.Status,
                                    req.OrderBy, req.IsDescending, req.PageNumber, req.PageSize);

        var events = await uOW.Events.GetPageAsync(spec);

        var eventResponses = events.Items.Select(eventEntity => eventEntity.ToResponse()).ToList();

        return new PageResult<EventRes>(
            eventResponses,
            events.PageNumber,
            events.PageSize,
            events.Count,
            events.TotalItems,
            events.TotalPages
        );
    }

    public async Task<IList<Location>> GetListLocation()
    {
        var locations = await uOW.Location.GetAllAsync();
        return locations.ToList();
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

    public async Task<EventRes> GetEvent(Guid eventId, Guid? userId)
    {
        // Flags for user-specific event details
        bool isRegistered = false;
        bool isReviewed = false;
        bool isOverlap = false;
        bool canReview = false;
        bool isCheckIn = false;

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
            var eventReview = await uOW.Reviews.GetListAsync(specReview);
            var eventOverlap = await uOW.EventRegistration.GetListAsync(specOverlap);
            var eventRegis = await uOW.EventRegistration.FindFirstOrDefaultAsync(specRegis);

            // Set flags based on results
            isReviewed = !eventReview.IsNullOrEmpty();
            isOverlap = !eventOverlap.IsNullOrEmpty();
            isRegistered = eventRegis is not null;
            isCheckIn = eventRegis?.IsCheckIn ?? false;

            // Logic to determine if the user can review
            if (_event.EndTime <= DateTime.Now && _event.EndTime >= DateTime.Now.AddDays(-2) &&
                eventRegis?.IsCheckIn == true)
            {
                canReview = true;
            }
        }

        // Return the event response
        return _event.ToResponse(isRegistered, isReviewed, isOverlap, canReview, isCheckIn);
    }

    public async Task<IList<EventRes>> GetListEventsByOrganizer(GetEventByOrganizerReq req)
    {
        var spec = new GetEventByOrganizerSpec(req.OrganizerId, req.Status);

        var _events = await uOW.Events.GetListAsync(spec);

        return _events.Select(e => e.ToResponse()).ToList();
    }

    public async Task<PageResult<EventRes>> GetListEventsOfOrganizer(GetEventOfOrganizerReq req)
    {
        var spec = new GetEventByOrganizerSpec(req.UserId, req.SearchKeyword, req.InMonth, req.InYear, req.EventTypes,
                                               req.EventTag, req.Status, req.PageNumber, req.PageSize);

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
        res = res.Where(r => userEvents.Any(ue => !(ue!.EventId == r.EventId))).ToList();

        return new PageResult<EventRes>(res, 1, 1, 1, 1, 1);
    }

    public async Task<IdRes> CreateEvent(CreateEventReq req, Guid organizerId)
    {
        var spec = new GetUserSpec(organizerId);

        var user = await uOW.Users.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(User));

        if (user.Verified != VerifiedStatus.Verified)
        {
            throw new UnauthorizedAccessException("This account not have permission.");
        }

        Event _event = req.ToEvent(organizerId);

        if (req.CreateFormDetailsReq is not null)
        {
            var formDetails = req.CreateFormDetailsReq.Select(f => new FormDetail(f.Name, f.Type, f.Options));
            var form = new Form
            {
                FormDetails = formDetails.ToList(),
                CreatedAt = DateTime.Now.AddHours(13),
            };

            _event.Form = form;
        }

        //await uOW.SaveChangesAsync();
        _event.Tags = [];
        foreach (var item in req.EventTags)
        {
            EventTag tag = new(_event.EventId, (string)item);
            _event.Tags.Add(tag);
            //await uOW.EventTag.AddAsync(tag);
        }
        //await uOW.SaveChangesAsync();

        if (req.Proposal is not null)
        {
            EventFile eventFile = new(req.Proposal, _event.EventId);
            _event.EventFile = eventFile;
            //await uOW.EventFile.AddAsync(eventFile);
        }

        EventMedia poster = new(_event.EventId, (int)MediaType.Poster, req.PosterImg);
        EventMedia thumbnail = new(_event.EventId, (int)MediaType.Thumbnail, req.ThumbnailImg);

        _event.EventMedias = [];
        _event.EventMedias.Add(poster);
        _event.EventMedias.Add(thumbnail);
        //await uOW.EventMedia.AddAsync(poster);
        //await uOW.EventMedia.AddAsync(thumbnail);

        await uOW.Events.AddAsync(_event);
        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task<IdRes> UpdateEvent(Guid id, Guid organizerId, UpdateEventReq req)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        if (_event.OrganizerId != organizerId)
            throw new UnauthorizedAccessException("You do not have permission to update this event.");

        // Update Form Details
        if (req.CreateFormDetailsReq is not null)
        {
            var formDetails = req.CreateFormDetailsReq.Select(f => new FormDetail(f.Name, f.Type, f.Options));
            _event.Form = new Form { FormDetails = formDetails.ToList() };
        }

        // Update Tags
        if (req.EventTags is not null)
        {
            var existingTags = await uOW.EventTag.GetListAsync(new GetEventTagSpec(id));
            if (existingTags != null)
            {
                foreach (var tag in existingTags)
                {
                    uOW.EventTag.Delete(tag);
                }
            }
            foreach (var item in req.EventTags)
            {
                var tag = new EventTag(_event.EventId, (string)item);
                await uOW.EventTag.AddAsync(tag);
            }
        }

        // Update Media
        if (req.PosterImg is not null)
        {
            var existingPoster = _event.EventMedias?.FirstOrDefault(m => m.MediaType == (int)MediaType.Poster);
            if (existingPoster != null)
            {
                existingPoster.MediaUrl = req.PosterImg;
            }
            else
            {
                _event.EventMedias.Add(new EventMedia(_event.EventId, (int)MediaType.Poster, req.PosterImg));
            }
        }

        if (req.ThumbnailImg is not null)
        {
            var existingThumbnail = _event.EventMedias?.FirstOrDefault(m => m.MediaType == (int)MediaType.Thumbnail);
            if (existingThumbnail != null)
            {
                existingThumbnail.MediaUrl = req.ThumbnailImg;
            }
            else
            {
                _event.EventMedias.Add(new EventMedia(_event.EventId, (int)MediaType.Thumbnail, req.ThumbnailImg));
            }
        }

        // Update Event File
        if (req.Proposal is not null)
        {
            _event.EventFile = new EventFile(req.Proposal, _event.EventId);
        }

        // Update Event Details
        _event.Update(req.EventName, req.Description, req.StartTime, req.EndTime, req.Location, req.LinkEvent,
                      req.PasswordMeeting, req.MaxAttendees, req.EventTypeId);

        if (uOW.IsUpdate(_event))
        {
            _event.UpdatedAt = DateTime.UtcNow;
        }

        // Save changes with concurrency handling
        try
        {
            await uOW.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception("The event was modified or deleted by another user.", ex);
        }

        return _event.EventId.ToResponse();
    }


    public async Task<IdRes> SubmitEvent(Guid id, Guid organizerId)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        if (_event.OrganizerId != organizerId)
            throw new UnauthorizedAccessException("Not have permission for submit Event");
        
        if (_event.Status == EventStatus.Draft || _event.Status == EventStatus.Rejected)
        {
            _event.Status = EventStatus.UnderReview;
            _event.UpdatedAt = DateTime.Now.AddHours(13);
        }

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task CancelEvent(Guid eventId, Guid organizerId)
    {
        var spec = new GetEventSpec(eventId);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));
        //Check event Owner
        if (_event.OrganizerId != organizerId)
        {
            throw new Exception("This event is not belong to you!");
        }

        // Enforce the "cancel before 2 days of start date" rule
        if (_event.StartTime <= DateTime.Now.AddDays(2))
        {
            throw new ValidationException("You can only cancel this event at least 2 days before the start date.");
        }

        // Retrieve and materialize participants
        var participants = _event.Registrations!; 

        if (participants.Any())
        {
            // Unregister all participants
            foreach (var participant in participants.ToList()) 
            {
                // Delete participant registration
                var regis = participant;
                uOW.EventRegistration.Delete(regis);

                // Update event max attendees
                if (_event.MaxAttendees != null)
                {
                    _event.MaxAttendees += 1;
                }

                // Send email notification
                var emailBody = EmailTemplates.ApologyForEventCancellationTemplate
                    .Replace("{userName}", participant.User!.Username)
                    .Replace("{eventName}", _event.EventName)
                    .Replace("{eventStartDate}", _event.StartTime.ToString("dd/MM/yyyy HH:mm"));

                await emailService.SendEmailAsync(participant.User!.Email, "Sự kiện bạn đăng kí đã bị hủy!", emailBody);
            }
        }

        var formSubmits = _event.FormSubmits!;

        if (formSubmits.Any())
        {
            foreach (var formSubmit in formSubmits.ToList())
            {
                var _formsubmit = formSubmit;
                uOW.FormSubmit.Delete(_formsubmit);
            }
        }

        // Update event status to Cancelled
        _event.Status = EventStatus.Cancelled;

        await uOW.SaveChangesAsync();

    }

    public async Task<IdRes> ApproveEvent(Guid id, bool isApproved, Guid userId, string processNote)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        var specSub = new GetUserSpec(userId);
        var _moderator = await uOW.Users.FindFirstOrDefaultAsync(specSub);

        if (_event.Status == EventStatus.UnderReview)
        {
            if (isApproved)
            {
                _event.Status = EventStatus.Upcoming;
                var notificationReq = new CreateNotificationReq(
                    _event.Organizer.UserId,
                    _event.EventId,
                    "Sự kiện của bạn đã được phê duyệt!", 
                    $"Sự kiện \"{_event.EventName}\" của bạn đã được phê duyệt. Hãy chuẩn bị thật tốt để tổ chức sự kiện thành công nhé!"
                );
                var notification = notificationReq.ToNotification();
                await uOW.Notification.AddAsync(notification);
            }
            else
            {
                _event.Status = EventStatus.Rejected;

                var notificationReq = new CreateNotificationReq(
                     _event.Organizer.UserId,
                    _event.EventId,
                    "Sự kiện của bạn đã bị từ chối", 
                    $"Rất tiếc, sự kiện \"{_event.EventName}\" của bạn đã không được phê duyệt. Vui lòng kiểm tra lại thông tin hoặc liên hệ ban tổ chức để biết thêm chi tiết."
                );

                var notification = notificationReq.ToNotification();
                await uOW.Notification.AddAsync(notification);

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

        var checkEvent = _event.Event
            ?? throw new NotFoundException(typeof(Event));

        if (checkEvent.Status != EventStatus.InProgress)
        {
            throw new ValidationException("Sự kiện đang chưa đến hạn/quá hạn checkin");
        }
        if (isOrganzier)
        {
            if (_event.IsCheckIn == true)
            {
                _event.IsCheckIn = false;
                _event.CheckinTime = null;
            }
            else
            {
                _event.IsCheckIn = true;
                _event.CheckinTime = DateTime.Now.AddHours(13);
            }
        }
        else
        {
            _event.IsCheckIn = true;
            _event.CheckinTime = DateTime.Now.AddHours(13);
        }
        await uOW.SaveChangesAsync();
    }

    public async Task DeleteEvent(Guid id)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        uOW.Events.Delete(_event);

        await uOW.SaveChangesAsync();
    }
    #endregion

    #region Event Registration
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

    public async Task<IList<EventRes>> GetRegisteredEvents(Guid userId,int? inMonth,int? inYear, bool isCompleted)
    {
        var spec = new GetRegisteredEventsSpec(userId, inMonth, inYear, isCompleted);
        var events = await uOW.Events.GetListAsync(spec);

        var res = events.Select(e => e.ToResponse()).ToList();

        return res;
    }
    #endregion

    #region Report
    public async Task<EventReportRes> Report(DateTime startDate, DateTime endDate)
    {
        var spec = new GetEventForReportSpec(startDate, endDate);

        var events = await uOW.Events.GetListAsync(spec);

        var noOfEvents = events.Count();
        var lstRegistered = events.Aggregate(new List<EventRegistration>(), (r, e) =>
        {
            e.Registrations.ForEach(r => r.Event = e);
            r.AddRange(e.Registrations!);
            return r;
        });

        var noOfUserAttended = lstRegistered.Where(r => r.IsCheckIn).GroupBy(r => r.UserId)
            .Select(g => new UserReportInfo(g.Key,
                                            g.Select(r => r.User!.Username).First(),
                                            g.Select(r => r.User!.AvatarUrl).First(),
                                            g.Select(r => r.EventId).Distinct().Count()));
        var noOfUserNotAttended = lstRegistered.Where(r => !r.IsCheckIn).GroupBy(r => r.UserId)
            .Select(g => new UserReportInfo(g.Key,
                                            g.Select(r => r.User!.Username).First(),
                                            g.Select(r => r.User!.AvatarUrl).First(),
                                            g.Select(r => r.EventId).Distinct().Count()));
        var test = lstRegistered.GroupBy(r => new { r.Event!.EndTime.Month, r.Event.EndTime.Year });

        var eventDetails = events.GroupBy(r => new { r.EndTime.Month, r.EndTime.Year })
            .Select(e => new EventReportDetailRes(e.Select(r => r.EventId).Distinct().Count(),
                                                  e.Key.Month,
                                                  e.Key.Year));

        var registrationDetail = lstRegistered.GroupBy(r => new { r.RegistrationTime.Month, r.RegistrationTime.Year })
            .Select(g => new RegistrationReportDetailInfo(g.Select(r => r.UserId).Count(), g.Key.Month, g.Key.Year));

        return new EventReportRes(noOfEvents, lstRegistered.Count, noOfUserAttended.Count(), noOfUserNotAttended.Count(),
                                  eventDetails.ToList(), noOfUserAttended.ToList(), noOfUserNotAttended.ToList(),
                                  registrationDetail.ToList());
    }

    public async Task<EventReportForOrgRes> ReportForOrganizer(Guid userId, DateTime startDate, DateTime endDate)
    {
        var spec = new GetEventForReportSpec(userId, startDate, endDate);

        var events = await uOW.Events.GetListAsync(spec);

        events = events.Take(10).ToList();

        var res = events.Select(e =>
        {
            var res = new EventReportDetailForOrgRes(e.EventId,
                                                     e.EventName,
                                                     e.Status.ToString(),
                                                     e.EventMedias!.Where(j => j.MediaType == 1).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
                                                     e.EventMedias!.Where(j => j.MediaType == 0).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
                                                     e.Registrations!.Count(),
                                                     e.Registrations!.Where(r => r.IsCheckIn).Count());
            return res;
        });

        return new EventReportForOrgRes(res.ToList());
    }

    public Task<EventReportDetailRes> ReportByEvent(Guid eventId)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Private function
    public async Task CreateNotification(CreateNotificationReq req)
    {
        var notification = req.ToNotification();

        await uOW.Notification.AddAsync(notification);
        await uOW.SaveChangesAsync();
    }
    #endregion 
}
