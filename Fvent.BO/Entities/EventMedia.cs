namespace Fvent.BO.Entities;

public class EventMedia
{
    public Guid EventMediaId { get; set; }
    public Guid EventId { get; set; }
    public int MediaType { get; set; }
    public string MediaUrl { get; set; }
    public DateTime UploadedAt { get; set; }

    public Event? Event { get; set; }

    public EventMedia(Guid eventId, int mediaType, string mediaUrl)
    {
        EventId = eventId;
        MediaType = mediaType;
        MediaUrl = mediaUrl;
        UploadedAt = DateTime.UtcNow;
    }
}
