﻿using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.EventSpec;
using static Fvent.Service.Specifications.EventTagSpec;

namespace Fvent.Service.Services.Imp;

public class EventService(IUnitOfWork uOW) : IEventService
{
    #region Student
    public async Task<PageResult<EventRes>> GetListRecommend(IdReq req)
    {
        var registerSpec = new GetListUserEventsSpec(req.Id);
        var userEventRegister = await uOW.EventRegistration.GetListAsync(registerSpec);
        var userEvents = userEventRegister.Select(r => r.Event);
        var eventTypes = userEvents.Select(e => e.EventTypeId).Distinct();
        var eventTags = userEvents.SelectMany(e => e.Tags!.Select(t => t.Tag)).Distinct();

        var eventSpec = new GetListRecommend(eventTypes, eventTags);
        Console.WriteLine("Hereeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
        var events = await uOW.Events.GetListAsync(eventSpec);

        var res = events.Select(e => e.ToResponse()).ToList();

        return new PageResult<EventRes>(res, 1, 1, 1, 1, 1);
    }
    #endregion

    #region CRUD Event
    public async Task<IdRes> CreateEvent(CreateEventReq req)
    {
        var _event = req.ToEvent();

        await uOW.Events.AddAsync(_event);
        await uOW.SaveChangesAsync();
        
        foreach (var item in req.eventTags)
        {
            EventTag tag = new EventTag(_event.EventId, (string)item);
            await uOW.EventTag.AddAsync(tag);
        }
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
        var spec = new GetEventSpec(req.SearchKeyword, req.FromDate, req.ToDate, req.EventType);

        // Apply sorting
        if (req.OrderBy == "Name")
        {
            spec.OrderBy(e => e.EventName, req.IsDescending);
        }
        else if (req.OrderBy == "StartTime")
        {
            spec.OrderBy(e => e.StartTime, req.IsDescending);
        }
        
        // Apply pagination
        spec.AddPagination(req.PageNumber, req.PageSize);

        // Get paginated list of events
        var _events = await uOW.Events.GetListAsync(spec);

        // Calculate the total number of items (without paging)
        var totalItems = _events.Count();

        // Map events to EventRes
        var eventResponses = _events.Select(e => e.ToResponse(
            e.Organizer!.FirstName + " " + e.Organizer!.LastName,
            e.EventType!.EventTypeName,
            null)).ToList();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);

        // Create and return PageResult
        return new PageResult<EventRes>(
            eventResponses,
            req.PageNumber,
            req.PageSize,
            eventResponses.Count,
            totalItems,
            totalPages
        );
    }

    /// <summary>
    /// Get Event Detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<EventRes> GetEvent(Guid id)
    {
        var spec = new GetEventSpec(id);
        var subSpec = new GetEventTagSpec(id);
        
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        var _eventTag = await uOW.EventTag.GetListAsync(subSpec)
            ?? throw new NotFoundException(typeof(Event));

        var eventTags = _eventTag.Select(e => e.Tag).ToList();

        return _event.ToResponse(_event.Organizer!.FirstName + " " + _event.Organizer!.LastName, _event.EventType!.EventTypeName, eventTags);
    }

    /// <summary>
    /// Update an Available Event
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<IdRes> UpdateEvent(Guid id, UpdateEventReq req)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        _event.Update(req.EventName,
            req.Description,
            req.StartTime,
            req.EndTime,
            req.Location,
            req.MaxAttendees,
            req.ProcessNote,
            req.OrganizerId,
            req.EventTypeId,
            req.StatusId);

        if (uOW.IsUpdate(_event))
            _event.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }

    public async Task<IList<EventRes>> GetListEventsByOrganizer(Guid organizerId)
    {
        var spec = new GetEventByOrganizerSpec(organizerId);

        var _events = await uOW.Events.GetListAsync(spec);

        return _events.Select(e => e.ToResponse(
                e.Organizer!.FirstName + " " + e.Organizer!.LastName,
                e.EventType!.EventTypeName,
                null)).ToList();
       
    }

    #endregion

    #region Event
    public async Task<EventRateRes> GetEventRate(IdReq req)
    {
        var spec = new GetEventRateSpec(req.Id);
        var reviews = await uOW.Reviews.GetListAsync(spec);

        double res = 0;
        
        if (!reviews.IsNullOrEmpty())
        {
            res = reviews.Sum(r => r.Rating)*1.0/reviews.Count();
        }

        return res.ToResponse();
    }
    #endregion

    #region Event-User
    public async Task<IList<UserRes>> GetEventRegisters(IdReq req)
    {
        var spec = new GetEventRegistersSpec(req.Id);
        var events = await uOW.Events.GetListAsync(spec);

        var users = events.SelectMany(e => e.Registrations)
            .Select(r => r.User);

        return users.Select(u => u.ToResponse<UserRes>()).ToList();
    }
    #endregion

    #region Event-Review
    public async Task<IdRes> CreateReview(CreateReviewReq req)
    {
        var review = req.ToReview();

        await uOW.Reviews.AddAsync(review);
        await uOW.SaveChangesAsync();

        return review.EventId.ToResponse();
    }

    public async Task<IList<ReviewRes>> GetEventReviews(IdReq req)
    {
        var spec = new GetEventReviewsSpec(req.Id);
        var reviews = await uOW.Reviews.GetListAsync(spec);

        return reviews.Select(r => r.ToReponse(r.User!.FirstName + " " + r.User.LastName)).ToList();
    }

    #endregion
}
