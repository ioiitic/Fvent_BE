namespace Fvent.Service.Request;

public record CreateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             int? MaxAttendees, string ProcessNote, Guid OrganizerId, Guid EventTypeId, int StatusId,
                             List<string> eventTags);

public record UpdateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             int? MaxAttendees, string ProcessNote, Guid OrganizerId, Guid EventTypeId, int StatusId);
public record GetEventsRequest(string? SearchKeyword, DateTime? FromDate, DateTime? ToDate, string? EventType,
                               string OrderBy = "Name", bool IsDescending = false, int PageNumber = 1, int PageSize = 9);