namespace Fvent.BO.Entities;

public class Conversation
{
    public Guid ConversationId { get; set; }
    public Guid EventId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Event? Event { get; set; }
}
