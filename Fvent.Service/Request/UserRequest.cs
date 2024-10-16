﻿using Fvent.BO.Enums;

namespace Fvent.Service.Request;

#region User
public record AuthReq(string Email, string Password);
#endregion

#region Admin
public record GetListUsersReq(string? Username, string? Email, string? RoleName, bool? Verified, string? OrderBy,
                              bool IsDescending = false, int PageNumber = 1, int PageSize = 9);
#endregion

public record CreateUserReq(string Username, string Email, string Password, string FirstName, string LastName,
                            string PhoneNumber, string Role);

public record UpdateUserReq(string Username, string AvatarUrl, string Email, string Password, string FirstName,
                            string LastName, string PhoneNumber, string CardUrl, string Campus);

public record ForgotPasswordReq(string email);

//public record ResetPasswordReq(Guid userId, string token, string newPassword);
