namespace Fvent.BO.Entities;

public class RefreshToken
{
    public int RefreshTokenId { get; set; }
    public required string Token { get; set; }
    public required string IpAddress { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}
