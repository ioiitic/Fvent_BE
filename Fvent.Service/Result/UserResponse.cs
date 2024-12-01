namespace Fvent.Service.Result;

public record GetListUserRes(Guid UserId, string Username, string AvatarUrl, string Email, string PhoneNumber, string StudentId,
                             string CardUrl, VerifiedStatus Verified, string RoleName, DateTime CreatedAt,
                             DateTime? UpdatedAt, bool IsDeleted, DateTime? DeletedAt);

public record UserRes(Guid UserId, string Username, string AvatarUrl, string Email, string PhoneNumber, string StudentId, string CardUrl, string VerifyStatus,string ProcessNote, bool? IsCheckin, bool? IsHaveUnreadNoti, string? RoleName);

public record AuthRes(string Token, string RefreshToken);

public record UserReportRes(int NoOfEvents, int NoOfOrganizers, IList<OrganizerReportInfo> Organizers);
public record OrganizerReportInfo(Guid UserId, string Username, string AvatarUrl, int NoOfEvents);