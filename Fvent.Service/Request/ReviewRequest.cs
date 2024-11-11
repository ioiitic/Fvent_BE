namespace Fvent.Service.Request;

public record CreateReviewReq(int Rating, string Comment);

public record UpdateReviewReq(int Rating, string Comment);