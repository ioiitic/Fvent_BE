using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;

public static class EventMapper
{
    public static Event ToEvent(
        this CreateEventReq src,
        Guid organizerId)
        => new(
            src.EventName,
            src.Description,
            src.StartTime,
            src.EndTime,
            src.LinkEvent,
            src.PasswordMeeting,
            src.Location,
            src.MaxAttendees,
            "",
            EventStatus.Draft,
            organizerId,
            "",
            src.EventTypeId,
            DateTime.Now);

    public static EventRes ToResponse(
        this Event src)
        => new(
            src.EventId,
            src.EventName,
            src.Description,
            src.StartTime,
            src.EndTime,
            src.Location,
            src.LinkEvent,
            src.PasswordMeeting,
            src.MaxAttendees,
            src.ProcessNote,
            src.OrganizerId,
            src.Organizer!.Username,
            src.EventType!.EventTypeId,
            src.EventType!.EventTypeName,
            src.EventMedias.Where(j => j.MediaType == 1).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.EventMedias.Where(j => j.MediaType == 0).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.Status.ToString(),
            null,
            null,
            null,
            null,
            src.EventFile.FileUrl,
            src.Tags.Select(t => t.Tag).ToList(),
            null);

    public static EventBannerRes ToBannerResponse(
        this Event src)
        => new(
            src.EventId,
            src.EventMedias.Where(j => j.MediaType == 1).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default");
    public static EventRes ToResponse(
        this Event src,
        bool isRegistered,
        bool isReviewed,
        bool isOverlap,
        bool canReview)
        => new(
            src.EventId,
            src.EventName,
            src.Description,
            src.StartTime,
            src.EndTime,
            src.Location,
            src.LinkEvent,
            src.PasswordMeeting,
            src.MaxAttendees,
            src.ProcessNote,
            src.OrganizerId,
            src.Organizer!.Username,
            src.EventType!.EventTypeId,
            src.EventType!.EventTypeName,
            src.EventMedias.Where(j => j.MediaType == 1).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.EventMedias.Where(j => j.MediaType == 0).Select(u => u.MediaUrl).FirstOrDefault() ?? "Default",
            src.Status.ToString(),
            isRegistered,
            isReviewed,
            isOverlap,
            canReview,
            src.EventFile.FileUrl,
            src.Tags.Select(t => t.Tag).ToList(),
            src.Form?.FormDetails?.Select(d => d.ToResponse()).ToList());

    public static EventRateRes ToResponse(this double src, int total )
        => new(src, total);

    public static FormDetailsRes ToResponse(this FormDetail src)
        => new(src.Name, src.Type, src.Options);
}