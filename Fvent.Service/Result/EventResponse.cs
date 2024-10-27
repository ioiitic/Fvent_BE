namespace Fvent.Service.Result;

public record EventRes(Guid EventId,
                       string EventName,
                       string Description,
                       DateTime StartTime,
                       DateTime EndTime,
                       string Location,
                       int? MaxAttendees,
                       string ProcessNote,
                       string OrganizerName,
                       string EventTypeName,
                       string PosterImg,
                       string ThumbnailImg,
                       EventStatus Status,
                       List<string> EventTags); 
public record EventRateRes(double AvgRate);