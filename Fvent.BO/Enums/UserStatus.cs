namespace Fvent.BO.Enums;

public enum UserStatus
{
    Pending = 0,      // User profile is awaiting review
    Accepted = 1,     // User has been reviewed and approved
    Rejected = 2      // User has been reviewed and not approved
}
