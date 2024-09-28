﻿namespace Fvent.BO.Entities;

public class EventFollower
{
    private DateTime utcNow;

    public EventFollower(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
        FollowTime = DateTime.UtcNow;
    }

    public Guid EventFollowerId { get; set; }
    public DateTime FollowTime { get; set; }

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Event Event { get; set; }
    public User User { get; set; }
}
