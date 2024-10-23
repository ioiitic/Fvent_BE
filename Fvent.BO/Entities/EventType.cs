namespace Fvent.BO.Entities;

public class EventType
{
    public EventType(string eventTypeName)
    {
        this.EventTypeName = eventTypeName;
    }

    public Guid EventTypeId { get; set; }
    public string EventTypeName { get; set; }

    public void Update(string newName)
    {
        EventTypeName = newName;
    }
}
