﻿namespace Fvent.BO.Entities;

public class EventFile
{
    public Guid EventFileId { get; set; }
    public string FileUrl { get; set; }
    public int FileType { get; set; }
    public DateTime UploadedAt { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; }
}
