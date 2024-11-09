using Fvent.BO.Common;

namespace Fvent.BO.Entities;

public class User : ISoftDelete
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string AvatarUrl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string CardUrl { get; set; }
    public bool EmailVerified { get; set; } = false;
    public VerifiedStatus Verified { get; set; }
    public string ProcessNote { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public IList<Comment>? Comments { get; set; }
    public IList<EventFollower>? Followers { get; set; }
    public IList<EventRegistration>? Registrations { get; set; }
    public IList<EventReview>? Reviews { get; set; }
    public IList<Notification>? Notifications { get; set; }
    public IList<Message>? Messages { get; set; }
    public IList<RefreshToken>? RefreshTokens { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public User(string username, string avatarUrl, string email, string password, 
                string phoneNumber, string cardUrl, int roleId, DateTime createdAt)
    {
        Username = username;
        AvatarUrl = avatarUrl;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        CardUrl = cardUrl;
        RoleId = roleId;
        CreatedAt = createdAt;
    }

    public void Update(string username, string avatarUrl, string phoneNumber)
    {
        Username = username;
        AvatarUrl = avatarUrl;
        PhoneNumber = phoneNumber;
    }
}
