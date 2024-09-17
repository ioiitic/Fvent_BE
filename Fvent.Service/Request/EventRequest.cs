namespace Fvent.Service.Request;

public record CreateEventReq(string EventName,
                             string Description,
                             DateTime StartTime,
                             DateTime EndTime,
                             string Location,
                             string Campus,
                             int? MaxAttendees,
                             decimal Price,
                             string ProcessNote,
                             Guid OrganizerId,
                             Guid EventTypeId,
                             int StatusId);

public record FollowEventReq(Guid userId);

public record UpdateEventReq(string EventName,
                             string Description,
                             DateTime StartTime,
                             DateTime EndTime,
                             string Location,
                             string Campus,
                             int? MaxAttendees,
                             decimal Price,
                             string ProcessNote,
                             Guid OrganizerId,
                             Guid EventTypeId,
                             int StatusId);