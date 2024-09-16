namespace Fvent.Service.Request;

public record CreateReviewReq(int Rating, string Comment, Guid EventId, Guid UserId);

public record UpdateReviewReq(int Rating, string Comment, Guid EventId, Guid UserId);