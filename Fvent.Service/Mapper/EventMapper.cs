using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;

public static class EventMapper
{
    public static Event ToEvent(
        this CreateEventReq src)
        => new(
            src.EventName,
            src.Description,
            src.StartTime,
            src.EndTime,
            src.Location,
            src.MaxAttendees,
            src.ProcessNote,
            src.OrganizerId,
            src.EventTypeId,
            src.StatusId,
            DateTime.UtcNow);

    public static EventRes ToResponse(
        this Event src)
        => new(
            src.EventId,
            src.EventName,
            src.Description,
            src.StartTime,
            src.EndTime,
            src.Location,
            src.MaxAttendees,
            src.ProcessNote,
            src.Organizer!.FirstName + " " + src.Organizer.LastName,
            src.EventType!.EventTypeName,
            src.EventMedia.Where( j=> j.MediaType == 1).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.EventMedia.Where(j => j.MediaType == 0).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.StatusId,
            src.Tags.Select(t => t.Tag).ToList());

    public static EventRes ToResponse(
        this Event src,
        string organizerName,
        string eventTypeName,
        List<string> eventTags)
        => new(
            src.EventId,
            src.EventName,
            src.Description,
            src.StartTime,
            src.EndTime,
            src.Location,
            src.MaxAttendees,
            src.ProcessNote,
            organizerName,
            eventTypeName,
            src.EventMedia.Where(j => j.MediaType == 1).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.EventMedia.Where(j => j.MediaType == 0).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.StatusId,
            eventTags);

    public static EventRateRes ToResponse(this double src)
        => new(src);
}