namespace Fvent.BO.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public Guid? EventId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public ReadStatus ReadStatus { get; set; }
        public DateTime SentTime { get; set; }

        public User? User { get; set; }
        public Event? Event { get; set; }

        public Notification() { }

        public Notification(Guid userId, Guid? eventId, string title, string message, ReadStatus status)
        {
            UserId = userId;
            EventId = eventId;
            Title = title;
            Message = message;
            ReadStatus = status;
            SentTime = DateTime.Now.AddHours(13);
        }
    }
}
