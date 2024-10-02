namespace Fvent.Service.Result;

public record EventRes(Guid eventId,
                       string EventName,
                       string Description,
                       DateTime StartTime,
                       DateTime EndTime,
                       string Location,
                       int? MaxAttendees,
                       string ProcessNote,
                       string OrganizerName,
                       string EventTypeName,
                       int StatusId,
                       List<string> EventTags); 
public record EventRateRes(double AvgRate);