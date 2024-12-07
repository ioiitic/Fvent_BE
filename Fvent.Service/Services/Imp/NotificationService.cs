using FirebaseAdmin.Messaging;
using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using Fvent.Service.Utils;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.NotificationSpec;
using static Fvent.Service.Specifications.UserSpec;

namespace Fvent.Service.Services.Imp;

public class NotificationService(IUnitOfWork uOW) : INotificationService
{
    //public async Task<IdRes> CreateNotification(CreateNotificationReq req)
    //{
    //    // Convert the request to a Notification entity
    //    var notification = req.ToNotification();

    //    // Save the notification in the database
    //    await uOW.Notification.AddAsync(notification);
    //    await uOW.SaveChangesAsync();

    //    // Fetch the target user (e.g., based on the event ID or target audience criteria)
    //    var user = await uOW.Users.FindFirstOrDefaultAsync(new GetUserSpec(req.userId));

    //    // Ensure the user has a valid Expo Push Token
    //    if (!string.IsNullOrEmpty(user?.FcmToken))
    //    {
    //        // Send the notification using ExpoNotificationService
    //        try
    //        {
    //            var title = req.title ?? "Thông báo từ hệ thống";
    //            var body = req.message ?? "Bạn có một thông báo mới.";
    //            var data = new
    //            {
    //                notificationId = notification.NotificationId,
    //                eventId = notification.EventId,
    //                userId = notification.UserId,
    //            };

    //            // Use ExpoNotificationService to send the notification
    //            await expoNotificationService.SendNotificationAsync(user.FcmToken, title, body, data);
    //            Console.WriteLine("Notification sent successfully.");
    //        }
    //        catch (Exception ex)
    //        {
    //            // Log or handle any errors that occur during notification sending
    //            Console.WriteLine($"Failed to send notification: {ex.Message}");
    //        }
    //    }
    //    else
    //    {
    //        // Log or handle the case when the user does not have a valid Expo Push Token
    //        Console.WriteLine("User does not have a valid Expo Push Token.");
    //    }

    //    // Return the event ID as the response
    //    return notification.EventId.ToResponse();
    //}

    public async Task<IdRes> CreateNotification(CreateNotificationReq req)
    {
        // Convert the request to a Notification entity
        var notification = req.ToNotification();

        // Save the notification in the database
        await uOW.Notification.AddAsync(notification);
        await uOW.SaveChangesAsync();

        // Fetch the target user (e.g., based on the event ID or target audience criteria)
        var user = await uOW.Users.FindFirstOrDefaultAsync(new GetUserSpec(req.userId));

        // Ensure the user has a valid Expo Push Token
        if (!string.IsNullOrEmpty(user?.FcmToken))
        {
            try
            {
                // Initialize FirebaseService with dynamic path to the service account key
                var serviceKeyPath = Path.Combine(AppContext.BaseDirectory, "firebase-service-key.json");
                var firebaseService = new FirebaseService(serviceKeyPath);

                // Send a single notification to the user
                await firebaseService.SendNotificationAsync(
                    user.FcmToken, 
                    notification.Title ?? "Tin khẩn !!!", 
                    notification.Message ?? "Bạn có bỏ lỡ gì ở Fvent không !!??"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("User does not have a valid Expo Push Token.");
        }

        // Return the event ID as the response
        return notification.NotificationId.ToResponse();
    }

    public async Task SendNotification(SendNotificationReq req)
    {

        var specUser = new GetUserByRoleSpec(req.role);
        var _users = await uOW.Users.GetListAsync(specUser);

        var fcmTokens = _users.Select(r => r.FcmToken).ToList();

        var userIds = _users.Select(r => r.UserId).ToList();


        foreach (var userId in userIds)
        {
            var notification = req.ToNotification(userId);
            await uOW.Notification.AddAsync(notification);
            await uOW.SaveChangesAsync();
        }    

        try
        {
            // Initialize FirebaseService with dynamic path to the service account key
            var serviceKeyPath = Path.Combine(AppContext.BaseDirectory, "firebase-service-key.json");
            var firebaseService = new FirebaseService(serviceKeyPath);

            await firebaseService.SendBulkNotificationsAsync(fcmTokens,
                                                             req.title,
                                                             req.message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }
    }

    public async Task DeleteNotification(Guid id)
    {
        var spec = new GetNotificationSpec(id);
        var notification = await uOW.Notification.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(BO.Entities.Notification));

        uOW.Notification.Delete(notification);

        await uOW.SaveChangesAsync();
    }

    public async Task<IdRes> ReadNotification(Guid id)
    {
        var spec = new GetNotificationSpec(id);
        var notification = await uOW.Notification.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(BO.Entities.Notification));
        var oldNotification = notification;
        notification.ReadStatus = ReadStatus.Read;

        uOW.Notification.Update(oldNotification, notification);

        await uOW.SaveChangesAsync();

        return notification.NotificationId.ToResponse();
    }

    public async Task ClearNotification(Guid userId)
    {
        var spec = new GetNotificationByUserSpec(userId);
        var notifications = await uOW.Notification.GetListAsync(spec);

        foreach (var notification in notifications)
        {
            uOW.Notification.Delete(notification);
        }

        await uOW.SaveChangesAsync();

    }

    public async Task<IList<NotificationRes>> GetListNotifications(Guid userId)
    {
        var spec = new GetNotificationByUserSpec(userId);
        var notifications = await uOW.Notification.GetListAsync(spec);

        return notifications.Select(r => r.ToReponse()).ToList();
    }

    public async Task<NotificationRes> GetNotification(Guid id)
    {
        var spec = new GetNotificationSpec(id);
        var notification = await uOW.Notification.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(BO.Entities.Notification));

        return notification.ToReponse();
    }


}
