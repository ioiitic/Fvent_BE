namespace Fvent.BO.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public string Message { get; set; }
        public int ReadStatus { get; set; } // Stored as an int in the database
        public DateTime SentTime { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }

        // Default constructor required for EF Core
        public Notification()
        {
            SentTime = DateTime.UtcNow;
        }

        // Static factory method for creating a new notification
        public static Notification Create(Guid userId, Guid eventId, string message, ReadStatus status)
        {
            return new Notification
            {
                UserId = userId,
                EventId = eventId,
                Message = message,
                ReadStatus = (int)status,
                SentTime = DateTime.UtcNow // Notification time is always set to now
            };
        }
    }
}
