namespace Fvent.BO.Entities;

public class EventType
{
    public Guid EventTypeId { get; set; }
    public string EventTypeName { get; set; }

    public EventType(string eventTypeName)
    {
        EventTypeName = eventTypeName;
    }

    public void Update(string newName)
    {
        EventTypeName = newName;
    }
}
