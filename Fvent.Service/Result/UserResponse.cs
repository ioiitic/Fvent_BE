namespace Fvent.Service.Result;

public record GetListUserRes(Guid UserId, string Username, string AvatarUrl, string Email, string PhoneNumber,
                             string CardUrl, VerifiedStatus Verified, string RoleName, DateTime CreatedAt,
                             DateTime? UpdatedAt, bool IsDeleted, DateTime? DeletedAt);

public record UserRes(Guid UserId, string Username, string AvatarUrl, string Email, string PhoneNumber, string CardUrl, string VerifyStatus,string ProcessNote, string? RoleName);

public record AuthRes(string Token, string RefreshToken);

public record UserReportRes(int NoOfEvents, int NoOfOrganizers, IList<OrganizerReportInfo> Organizers);
public record OrganizerReportInfo(Guid UserId, string Username, string AvatarUrl, int NoOfEvents);