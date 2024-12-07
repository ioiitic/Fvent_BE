﻿namespace Fvent.Service.Result;

public record EventRes(Guid EventId, string EventName, string Description, DateTime StartTime, DateTime EndTime,
                       string Location, string LinkEvent, string PasswordMeeting, int? MaxAttendees, string ProcessNote,
                       Guid OrganizerId, string OrganizerName, Guid EventTypeId, string EventTypeName, string PosterImg,
                       string ThumbnailImg, string Status, bool? IsRegistered, bool? IsReviewed, bool? IsOverlap,
                       bool? IsCheckIn, bool? CanReview, string Proposal, List<string> EventTags,
                       IList<FormDetailsRes>? Form);
public record EventRateRes(double AvgRate, int TotalRate);

public record EventBannerRes(Guid EventId, string PosterImg);

public record FormDetailsRes(string Name, string Type, IList<string> Options);

public record UserReportInfo(Guid UserId, string Username, string AvatarUrl, int NoOfEvents);
public record EventReportDetailRes(int NoOfEvents, int Month, int Year);
public record RegistrationReportDetailInfo(int NoOfRegistered, int Month, int Year);
public record EventReportRes(int NoOfEvents, int NoOfRegistered, int NoOfUsersAttended, int NoOfUsersNotAttended,
                             IList<EventReportDetailRes> Details, IList<UserReportInfo> UsersAttended,
                             IList<UserReportInfo> UsersNotAttended,
                             IList<RegistrationReportDetailInfo> RegistrationDetails);