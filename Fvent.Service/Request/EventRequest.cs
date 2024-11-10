﻿namespace Fvent.Service.Request;

public record CreateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             string? LinkEvent, string? PasswordMeeting, int? MaxAttendees,
                             IList<CreateFormDetailReq>? CreateFormDetailsReq, Guid EventTypeId,
                             string PosterImg, string ThumbnailImg, List<string> EventTags);

public record CreateFormDetailReq(string name, string type, IList<string> options);

public record UpdateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             string? LinkEvent, string? PasswordMeeting, int? MaxAttendees, string ProcessNote,
                             EventStatus Status, Guid EventTypeId);
public record GetEventsRequest(string? SearchKeyword, int? InMonth, int? InYear, List<string>? EventTypes, string? EventTag,string? Status,
                               string OrderBy = "StartTime", bool IsDescending = false, int PageNumber = 1, int PageSize = 9);
public record ApproveEventRequest(string ProcessNote);
public record GetEventByOrganizerReq(Guid OrganizerId, string? Status);