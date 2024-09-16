namespace Fvent.BO.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public string Message { get; set; }
    public int ReadStatus { get; set; }
    public DateTime SentTime { get; set; }

    public User User { get; set; }
    public Event Event { get; set; }
}
