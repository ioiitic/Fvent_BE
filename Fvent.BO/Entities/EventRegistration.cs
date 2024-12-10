namespace Fvent.BO.Entities;

public class EventRegistration
{
    public Guid EventRegistrationId { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public bool IsCheckIn { get; set; }
    public DateTime RegistrationTime { get; set; }
    public DateTime? CheckinTime { get; set; }
    public bool IsReminderSent60 { get; set; } 
    public bool IsReminderSent30 { get; set; }

    public Event? Event { get; set; }
    public User? User { get; set; }

    public EventRegistration(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
        RegistrationTime = DateTime.Now.AddHours(13);
        CheckinTime = null;
        IsReminderSent60 = false;
        IsReminderSent30 = false;
    }
}
