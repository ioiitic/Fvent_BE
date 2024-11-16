namespace Fvent.Service.Result;

public record EventRes(Guid EventId, string EventName, string Description, DateTime StartTime, DateTime EndTime,
                       string Location, string LinkEvent, string PasswordMeeting, int? MaxAttendees, string ProcessNote,
                       Guid OrganizerId, string OrganizerName, string EventTypeName, string PosterImg,
                       string ThumbnailImg, string Status, bool? IsRegistered, List<string> EventTags,
                       IList<FormDetailsRes>? Form);
public record EventRateRes(double AvgRate, int TotalRate);

public record EventBannerRes(Guid EventId, string PosterImg);

public record FormDetailsRes(string Name, string Type, IList<string> Options);