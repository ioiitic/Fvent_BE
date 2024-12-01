public class VerificationToken
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }

    public VerificationToken(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
        ExpiryDate = DateTime.UtcNow.AddHours(24); 
    }
    public VerificationToken(Guid userId, string token, DateTime expireDate)
    {
        UserId = userId;
        Token = token;
        ExpiryDate = expireDate;
    }
}

