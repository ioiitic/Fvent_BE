using Fvent.BO.Common;
using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Fvent.Service.Specifications;
using System.Diagnostics.Tracing;
using static Fvent.Service.Specifications.EventFollowerSpec;
using static Fvent.Service.Specifications.EventSpec;
using static Fvent.Service.Specifications.EventTagSpec;
using static Fvent.Service.Specifications.ReviewSpec;

namespace Fvent.Service.Services.Imp;

public class EventService(IUnitOfWork uOW) : IEventService
{
    /// <summary>
    /// Create new Event
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Delete An Event
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task DeleteEvent(Guid id)
    {
        var spec = new GetEventSpec(id);
        var _event = await uOW.Events.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Event));

        uOW.Events.Delete(_event);

        await uOW.SaveChangesAsync();
    }

    /// <summary>
    /// Get List Event with search, sort, filter, paging
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<PageResult<EventRes>> GetListEvents(GetEventsRequest req)
    {
        var spec = new GetEventSpec(req.SearchKeyword, req.Campus, req.FromDate, req.ToDate, req.EventType);

        // Apply sorting
        if (req.OrderBy == "Name")
        {
            spec.OrderBy(e => e.EventName, req.IsDescending);
        }
        else if (req.OrderBy == "StartTime")
        {
            spec.OrderBy(e => e.StartTime, req.IsDescending);
        }
        // Add additional sorting options here as needed

        
        // Apply pagination
        spec.AddPagination(req.PageNumber, req.PageSize);

        // Get paginated list of events
        var _events = await uOW.Events.GetListAsync(spec);

        // Calculate the total number of items (without paging)
        var totalItems = _events.Count();

        // Map events to EventRes
        var eventResponses = _events.Select(e => e.ToReponse(
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

        return _event.ToReponse(_event.Organizer!.FirstName + " " + _event.Organizer!.LastName, _event.EventType!.EventTypeName, eventTags);
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
            req.Campus,
            req.MaxAttendees,
            req.Price,
            req.ProcessNote,
            req.OrganizerId,
            req.EventTypeId,
            req.StatusId);

        if (uOW.IsUpdate(_event))
            _event.UpdatedAt = DateTime.UtcNow;

        await uOW.SaveChangesAsync();

        return _event.EventId.ToResponse();
    }
}
