namespace Fvent.BO.Enums;

public enum EventStatus
{
    Draft = 0,           // Event details are being drafted
    UnderReview = 1,     // Submitted for review and approval
    Rejected = 2,        // Event did not pass approval
    Upcoming = 3,        // Approved and set to take place soon
    InProgress = 4,      // Event has started and is ongoing
    Completed = 5,       // Event has successfully ended
    Cancelled = 6        // Event was canceled before completion
}