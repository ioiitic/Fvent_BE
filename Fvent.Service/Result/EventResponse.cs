namespace Fvent.Service.Result;

public record EventRes(string EventName,
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
                       int StatusId);
public record EventRateRes(double AvgRate);