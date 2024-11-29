namespace Fvent.Service.Result;

public record EventRes(Guid EventId, string EventName, string Description, DateTime StartTime, DateTime EndTime,
                       string Location, string LinkEvent, string PasswordMeeting, int? MaxAttendees, string ProcessNote,
                       Guid OrganizerId, string OrganizerName, Guid EventTypeId, string EventTypeName, string PosterImg,
                       string ThumbnailImg, string Status, bool? IsRegistered,bool? IsReviewed,bool? IsOverlap,bool? CanReview, string proposal, List<string> EventTags,
                       IList<FormDetailsRes>? Form);
public record EventRateRes(double AvgRate, int TotalRate);

public record EventBannerRes(Guid EventId, string PosterImg);

public record FormDetailsRes(string Name, string Type, IList<string> Options);