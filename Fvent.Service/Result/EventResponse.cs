namespace Fvent.Service.Result;

public record EventRes(Guid EventId, string EventName, string Description, DateTime StartTime, DateTime EndTime,
                       string Location, string LinkEvent, string PasswordMeeting, int? MaxAttendees, string ProcessNote,
                       Guid OrganizerId, string OrganizerName, string EventTypeName, string PosterImg,
                       string ThumbnailImg, string Status, bool? IsRegistered, IList<string> EventTags,
                       IList<FormDetailsRes>? Form);
public record EventRateRes(double AvgRate);

public record FormDetailsRes(string Name, string Type, IList<string> Options);

public record EventReportDetailRes(int NoOfUsers, IList<UserRes> UsersAttended, IList<UserRes> UsersAbsented);
public record EventReportRes(int NoOfUsers, int NoOfEvents);