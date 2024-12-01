using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Repository.Data;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Microsoft.Extensions.Hosting;
using static Fvent.Service.Specifications.EventSpec;

namespace Fvent.Service.Services.Imp;

public class MyBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            MyDbContext context = new MyDbContext();
            IUnitOfWork uOW = new UnitOfWork(context);

            var serviceKeyPath = Path.Combine(AppContext.BaseDirectory, "firebase-service-key.json");
            var firebaseService = new FirebaseService(serviceKeyPath);

            // Step 1: Send reminders
            await SendRemindersAsync(uOW, firebaseService);

            // Step 2: Update event statuses
            await UpdateEventStatusesAsync(uOW, firebaseService);

            // Step 3: Cleanup expired tokens
            await CleanupExpiredTokensAsync(uOW);

            // Wait for the next iteration
            await Task.Delay(30000);
        }
    }

    private async Task SendRemindersAsync(IUnitOfWork uOW, FirebaseService firebaseService)
    {
        // Reminder 1: 1 hour before the event
        await ProcessReminders(uOW, firebaseService, 60,
            "Sự kiện sắp diễn ra",
            "Sự kiện '{0}' bạn đã đăng ký sẽ bắt đầu sau 1 giờ. Hãy sẵn sàng!");

        // Reminder 2: 30 minutes before the event
        await ProcessReminders(uOW, firebaseService, 30,
            "Sự kiện sắp diễn ra",
            "Sự kiện '{0}' bạn đã đăng ký sẽ bắt đầu sau 30 phút. Đừng bỏ lỡ!");
    }

    private async Task ProcessReminders(
        IUnitOfWork uOW,
        FirebaseService firebaseService,
        int reminderThresholdMinutes,
        string notificationTitle,
        string notificationMessageTemplate)
    {
        var reminderSpec = new GetEventReminderSpec(reminderThresholdMinutes);
        var events = await uOW.Events.GetListAsync(reminderSpec);

        foreach (var _event in events)
        {
            var spec = new GetRegisteredUsersSpec(_event.EventId);
            var registeredUsers = await uOW.Events.GetListAsync(spec);

            // Extract tokens and user IDs
            var fcmTokens = registeredUsers
                .SelectMany(e => e.Registrations!)
                .Select(r => r.User!.FcmToken)
                .ToList();

            var userIds = registeredUsers
                .SelectMany(e => e.Registrations!)
                .Select(r => r.User!.UserId)
                .ToList();

            // Create notifications and send them
            foreach (var userId in userIds)
            {
                var notificationReq = new CreateNotificationReq(
                    userId,
                    _event.EventId,
                    notificationTitle,
                    string.Format(notificationMessageTemplate, _event.EventName)
                );

                var notification = notificationReq.ToNotification();
                await uOW.Notification.AddAsync(notification);
                await uOW.SaveChangesAsync();
            }

            // Send bulk notifications via Firebase
            await firebaseService.SendBulkNotificationsAsync(
                fcmTokens,
                notificationTitle,
                string.Format(notificationMessageTemplate, _event.EventName)
            );
        }

        await uOW.SaveChangesAsync();
    }

    private async Task UpdateEventStatusesAsync(IUnitOfWork uOW, FirebaseService firebaseService)
    {
        var events = await uOW.Events.GetAllAsync();

        foreach (Event _event in events)
        {
            if (_event.Status == EventStatus.Upcoming || _event.Status == EventStatus.InProgress)
            {
                if (_event.EndTime >= DateTime.Now.AddHours(13) && _event.StartTime <= DateTime.Now.AddHours(13))
                {
                    _event.Update(EventStatus.InProgress);
                }
                else if (_event.EndTime < DateTime.Now.AddHours(13))
                {
                    _event.Update(EventStatus.Completed);

                    // Get all registered users for the event
                    var spec = new GetRegisteredUsersSpec(_event.EventId);
                    var listEvents = await uOW.Events.GetListAsync(spec);

                    // Separate users who checked in and those who didn't
                    var checkedInUsers = listEvents
                        .SelectMany(e => e.Registrations!)
                        .Where(r => r.IsCheckIn)
                        .Select(r => r.User!)
                        .ToList();

                    var notCheckedInUsers = listEvents
                        .SelectMany(e => e.Registrations!)
                        .Where(r => !r.IsCheckIn)
                        .Select(r => r.User!)
                        .ToList();

                    // Notify users who checked in
                    if (checkedInUsers.Any())
                    {
                        var fcmTokensCheckedIn = checkedInUsers.Select(u => u.FcmToken).ToList();

                        foreach (var user in checkedInUsers)
                        {
                            var notificationReq = new CreateNotificationReq(
                                user.UserId,
                                _event.EventId,
                                "Sự kiện bạn đã tham gia vừa kết thúc",
                                $"Hãy để lại cho sự kiện '{_event.EventName}' của chúng tôi vài lời đánh giá nhé!");

                            var notification = notificationReq.ToNotification();
                            await uOW.Notification.AddAsync(notification);
                            await uOW.SaveChangesAsync();
                        }

                        await firebaseService.SendBulkNotificationsAsync(
                            fcmTokensCheckedIn,
                            "Sự kiện bạn đã tham gia vừa kết thúc",
                            $"Hãy để lại cho sự kiện '{_event.EventName}' của chúng tôi vài lời đánh giá nhé!");
                    }

                    // Notify users who didn't check in
                    if (notCheckedInUsers.Any())
                    {
                        var fcmTokensNotCheckedIn = notCheckedInUsers.Select(u => u.FcmToken).ToList();

                        foreach (var user in notCheckedInUsers)
                        {
                            var notificationReq = new CreateNotificationReq(
                                user.UserId,
                                _event.EventId,
                                "Bạn đã bỏ lỡ một sự kiện!!!",
                                $"Bạn đã không check-in vào sự kiện '{_event.EventName}'. Hãy chắc chắn không bỏ lỡ lần tới!");

                            var notification = notificationReq.ToNotification();
                            await uOW.Notification.AddAsync(notification);
                            await uOW.SaveChangesAsync();
                        }

                        await firebaseService.SendBulkNotificationsAsync(
                            fcmTokensNotCheckedIn,
                            "Bạn đã bỏ lỡ sự kiện",
                            $"Bạn đã không check-in vào sự kiện '{_event.EventName}'. Hãy chắc chắn không bỏ lỡ lần tới!");
                    }
                }
            }
        }

        await uOW.SaveChangesAsync();
    }


    private async Task CleanupExpiredTokensAsync(IUnitOfWork uOW)
    {
        var verificationTokens = await uOW.VerificationToken.GetAllAsync();
        foreach (var verification in verificationTokens)
        {
            if (verification.ExpiryDate < DateTime.Now.AddHours(13))
                uOW.VerificationToken.Delete(verification);
        }

        var refreshTokens = await uOW.RefreshToken.GetAllAsync();
        foreach (var refresh in refreshTokens)
        {
            if (refresh.Expires < DateTime.Now.AddHours(13))
                uOW.RefreshToken.Delete(refresh);
        }

        await uOW.SaveChangesAsync();
    }
}
