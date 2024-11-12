namespace Fvent.Service.Result;

public record ReviewRes(Guid reviewId, int Rating, string Comment, string Fullname, string avatar, DateTime reviewDate);
