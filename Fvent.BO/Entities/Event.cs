using Fvent.BO.Common;

namespace Fvent.BO.Entities;

public class Event : ISoftDelete
{
    public Guid EventId { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public string Campus { get; set; }
    public int? MaxAttendees { get; set; }
    public decimal Price { get; set; } = 0.00m;
    public string ProcessNote { get; set; }

    public Guid OrganizerId { get; set; }
    public Guid EventTypeId { get; set; }
    public int StatusId { get; set; }
    public User? Organizer { get; set; }
    public EventType? EventType { get; set; }
    public EventStatus? Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Event(string eventName,
                 string description,
                 DateTime startTime,
                 DateTime endTime,
                 string location,
                 string campus,
                 int? maxAttendees,
                 decimal price,
                 string processNote,
                 Guid organizerId,
                 Guid eventTypeId,
                 int statusId,
                 DateTime createdAt)
    {
        EventName = eventName;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Location = location;
        Campus = campus;
        MaxAttendees = maxAttendees;
        Price = price;
        ProcessNote = processNote;
        OrganizerId = organizerId;
        EventTypeId = eventTypeId;
        StatusId = statusId;
        CreatedAt = createdAt;
    }

    public void Update(string eventName,
                       string description,
                       DateTime startTime,
                       DateTime endTime,
                       string location,
                       string campus,
                       int? maxAttendees,
                       decimal price,
                       string processNote,
                       Guid organizerId,
                       Guid eventTypeId,
                       int statusId)
    {
        EventName = eventName;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Location = location;
        Campus = campus;
        MaxAttendees = maxAttendees;
        Price = price;
        ProcessNote = processNote;
        OrganizerId = organizerId;
        EventTypeId = eventTypeId;
        StatusId = statusId;
    }
}
