namespace Fvent.Service.Request;

public record CreateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             int? MaxAttendees, string ProcessNote, EventStatus Status, Guid OrganizerId, Guid EventTypeId,
                             string PosterImg, string ThumbnailImg, List<string> EventTags);

public record UpdateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             int? MaxAttendees, string ProcessNote, EventStatus Status, Guid OrganizerId, Guid EventTypeId);
public record GetEventsRequest(string? SearchKeyword, DateTime? FromDate, DateTime? ToDate, string? EventType,
                               string OrderBy = "Name", bool IsDescending = false, int PageNumber = 1, int PageSize = 9);