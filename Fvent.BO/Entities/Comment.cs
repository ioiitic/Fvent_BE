using Fvent.BO.Common;

namespace Fvent.BO.Entities;

public class Comment : ISoftDelete
{
    public Guid CommentId { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; }

    public Event? Event { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Comment(Guid eventId, Guid userId, string commentText, DateTime createdAt)
    {
        EventId = eventId;
        UserId = userId;
        CommentText = commentText;
        CreatedAt = createdAt;
    }
}
