namespace Fvent.Service.Result;

public record EventRes(Guid EventId,
                       string EventName,
                       string Description,
                       DateTime StartTime,
                       DateTime EndTime,
                       string Location,
                       string LinkEvent,
                       string PasswordMeeting,
                       int? MaxAttendees,
                       string ProcessNote,
                       string OrganizerName,
                       string EventTypeName,
                       string PosterImg,
                       string ThumbnailImg,
                       string Status,
                       List<string> EventTags); 
public record EventRateRes(double AvgRate);