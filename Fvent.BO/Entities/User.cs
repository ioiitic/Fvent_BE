using Fvent.BO.Common;

namespace Fvent.BO.Entities;

public class User : ISoftDelete
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string AvatarUrl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string CardUrl { get; set; }
    public bool Verified { get; set; } = false;

    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public IList<Comment>? Comments { get; set; }
    public IList<EventFollower>? Followers { get; set; }
    public IList<EventRegistration>? Registrations { get; set; }
    public IList<EventReview>? Reviews { get; set; }
    public IList<Notification>? Notifications { get; set; }
    public IList<Message>? Messages { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public User(string username,
                string avatarUrl,
                string email,
                string password,
                string firstName,
                string lastName,
                string phoneNumber,
                string cardUrl,
                int roleId,
                DateTime createdAt)
    {
        Username = username;
        AvatarUrl = avatarUrl;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        CardUrl = cardUrl;
        RoleId = roleId;
        CreatedAt = createdAt;
    }

    public void Update(string username,
                       string avatarUrl,
                       string email,
                       string password,
                       string firstName,
                       string lastName,
                       string phoneNumber,
                       string cardUrl)
    {
        Username = username;
        AvatarUrl = avatarUrl;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        CardUrl = cardUrl;
    }
}
