using Fvent.BO.Common;
using Fvent.BO.Enums;

namespace Fvent.BO.Entities;

public class Event : ISoftDelete
{
    public Guid EventId { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? LinkEvent { get; set; }
    public string? PasswordMeeting { get; set; }
    public string Location { get; set; }
    public int? MaxAttendees { get; set; }
    public string ProcessNote { get; set; }
    public EventStatus Status { get; set; }
    public Guid OrganizerId { get; set; }
    public Guid EventTypeId { get; set; }

    public User? Organizer { get; set; }
    public EventType? EventType { get; set; }
    public Form? Form { get; set; }
    public IList<EventRegistration>? Registrations { get; set; }
    public IList<EventTag>? Tags { get; set; }
    public IList<EventMedia>? EventMedias { get; set; }
    public EventFile? EventFile { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Event(string eventName, string description, DateTime startTime, DateTime endTime, string linkEvent, string passwordMeeting, string location,
                 int? maxAttendees, string processNote, EventStatus status, Guid organizerId, Guid eventTypeId,
                 DateTime createdAt)
    {
        EventName = eventName;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        LinkEvent = linkEvent;
        PasswordMeeting = passwordMeeting;
        Location = location;
        MaxAttendees = maxAttendees;
        ProcessNote = processNote;
        OrganizerId = organizerId;
        EventTypeId = eventTypeId;
        Status = status;
        CreatedAt = createdAt;
    }

    public void Update(EventStatus status)
    {
        Status = status;
    }

    public void Update(string eventName, string description, DateTime startTime, DateTime endTime, string location,
                       int? maxAttendees, string processNote, EventStatus status, Guid organizerId, Guid eventTypeId)
    {
        EventName = eventName;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Location = location;
        MaxAttendees = maxAttendees;
        ProcessNote = processNote;
        OrganizerId = organizerId;
        EventTypeId = eventTypeId;
        Status = status;
    }
}
