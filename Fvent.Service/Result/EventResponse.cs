namespace Fvent.Service.Result;

public record EventRes(Guid eventId,
                       string EventName,
                       string Description,
                       DateTime StartTime,
                       DateTime EndTime,
                       string Location,
                       string Campus,
                       int? MaxAttendees,
                       decimal Price,
                       string ProcessNote,
                       string OrganizerName,
                       string EventTypeName,
                       int StatusId,
                       List<string> eventTags); 
public record EventRateRes(double AvgRate);