namespace Fvent.BO.Entities;

public class EventRegistration
{
    public EventRegistration(Guid eventId, Guid userId)
    {
        EventId=eventId;
        UserId=userId;
        PaymentStatus = 0;
        RegistrationTime = DateTime.UtcNow;
    }

    public Guid EventRegistrationId { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public int PaymentStatus { get; set; }
    public DateTime RegistrationTime { get; set; }

    public Event Event { get; set; }
    public User User { get; set; }
}
