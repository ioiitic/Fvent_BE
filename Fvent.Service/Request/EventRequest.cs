﻿namespace Fvent.Service.Request;

public record CreateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,string? LinkEvent, string? PasswordMeeting,
                             int? MaxAttendees, string ProcessNote, Guid OrganizerId, Guid EventTypeId,
                             int StatusId, string posterImg, string thumbnailImg, List<string> eventTags);

public record UpdateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location, string? LinkEvent, string? PasswordMeeting,
                             int? MaxAttendees, string ProcessNote, Guid OrganizerId, Guid EventTypeId, int StatusId);
public record GetEventsRequest(string? SearchKeyword, int? InMonth,int? InYear,string? EventType, string? EventTag,
                               string OrderBy = "StartTime", bool IsDescending = false, int PageNumber = 1, int PageSize = 9);