namespace Fvent.BO.Entities;

public class EventFile
{
    public Guid EventFileId { get; set; }
    public string FileUrl { get; set; }
    public int? FileType { get; set; }
    public DateTime UploadedAt { get; set; }

    public Guid EventId { get; set; }

    public EventFile(string fileUrl, Guid eventId)
    {
        FileUrl = fileUrl;
        EventId = eventId;
        FileType = 0;
        UploadedAt = DateTime.Now;
    }
}
