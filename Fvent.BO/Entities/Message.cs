namespace Fvent.BO.Entities;

public class Message
{
    public int MessageId { get; set; }
    public int ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string MessageText { get; set; }
    public DateTime SentTime { get; set; }

    public Conversation Conversation { get; set; }
    public User Sender { get; set; }
}
