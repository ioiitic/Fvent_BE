namespace Fvent.BO.Entities;

public class EventTag
{
    public EventTag(Guid eventId, string tag)
    {
        EventId = eventId;
        Tag = tag;  
    }

    public Guid EventTagId { get; set; }
    public Guid EventId { get; set; }
    public string Tag { get; set; }

    public Event Event { get; set; }
}
