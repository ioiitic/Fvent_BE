using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}

