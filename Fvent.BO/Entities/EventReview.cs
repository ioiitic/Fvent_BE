using Fvent.BO.Common;

namespace Fvent.BO.Entities;

public class EventReview : ISoftDelete
{
    public Guid EventReviewId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Event? Event { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public EventReview(int rating, string comment, Guid eventId, Guid userId, DateTime createdAt)
    {
        Rating = rating;
        Comment = comment;
        EventId = eventId;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public void Update(int rating, string comment, Guid eventId, Guid userId)
    {
        Rating = rating;
        Comment = comment;
        EventId = eventId;
    }
}
