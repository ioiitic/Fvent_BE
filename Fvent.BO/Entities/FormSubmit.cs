namespace Fvent.BO.Entities;

public class FormSubmit
{
    public Guid FormSubmitId { get; set; }

    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public string Data { get; set; }

    public User? User { get; set; }
    public Event? Event { get; set; }

    public FormSubmit(Guid userId, Guid eventId, string data)
    {
        UserId = userId;
        EventId = eventId;
        Data = data;
    }
}
