using Fvent.BO.Common;

namespace Fvent.BO.Entities;

public class EventType :ISoftDelete
{
    public Guid EventTypeId { get; set; }
    public string EventTypeName { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public EventType(string eventTypeName)
    {
        EventTypeName = eventTypeName;
    }

    public void Update(string newName)
    {
        EventTypeName = newName;
    }
}
