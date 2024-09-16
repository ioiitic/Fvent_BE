namespace Fvent.BO.Entities;

public class EventMedia
{
    public Guid EventMediaId { get; set; }
    public Guid EventId { get; set; }
    public int MediaType { get; set; }
    public string MediaUrl { get; set; }
    public DateTime UploadedAt { get; set; }

    public Event Event { get; set; }
}
