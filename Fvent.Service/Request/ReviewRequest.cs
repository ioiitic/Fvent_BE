namespace Fvent.Service.Request;

public record CreateReviewReq(int Rating, string Comment, Guid UserId);

public record UpdateReviewReq(int Rating, string Comment, Guid UserId);