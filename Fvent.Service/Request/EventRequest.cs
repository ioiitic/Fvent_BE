using Fvent.BO.Enums;
using System.Diagnostics.Tracing;

namespace Fvent.Service.Request;

public record CreateEventReq(string EventName, string Description, DateTime StartTime, DateTime EndTime, string Location,
                             string? LinkEvent, string? PasswordMeeting, int? MaxAttendees,
                             IList<CreateFormDetailReq>? CreateFormDetailsReq, Guid EventTypeId, string PosterImg,
                             string ThumbnailImg, List<string> EventTags, string Proposal);

public record CreateFormDetailReq(string name, string type, IList<string> options);

public record UpdateEventReq(string? EventName, string? Description, DateTime? StartTime, DateTime? EndTime, string? Location,
                             string? LinkEvent, string? PasswordMeeting, int? MaxAttendees,
                             IList<CreateFormDetailReq>? CreateFormDetailsReq, string? PosterImg, string? ThumbnailImg,
                             Guid? EventTypeId, List<string>? EventTags, string? Proposal);
public record GetEventsRequest(string? SearchKeyword, int? InMonth, int? InYear, List<string>? EventTypes,
                               string? EventTag, string? Status, string OrderBy = "StartTime", bool IsDescending = false,
                               int PageNumber = 1, int PageSize = 9);
public record ApproveEventRequest(string ProcessNote);
public record GetEventByOrganizerReq(Guid OrganizerId, string? searchKeyword, string? Status);