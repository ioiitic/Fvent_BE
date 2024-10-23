using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using static Fvent.Service.Specifications.NotificationSpec;

namespace Fvent.Service.Services.Imp;

public class NotificationService(IUnitOfWork uOW) : INotificationService
{
    public async Task<IdRes> CreateNotification(CreateNotificationReq req)
    {
        var notification = req.ToNotification();

        await uOW.Notification.AddAsync(notification);
        await uOW.SaveChangesAsync();

        return notification.EventId.ToResponse();
    }

    public async Task DeleteNotification(Guid id)
    {
        var spec = new GetNotificationSpec(id);
        var notification = await uOW.Notification.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Notification));

        uOW.Notification.Delete(notification);

        await uOW.SaveChangesAsync();
    }

    public async Task<IdRes> ReadNotification(Guid id)
    {
        var spec = new GetNotificationSpec(id);
        var notification = await uOW.Notification.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(Notification));
        var oldNotification = notification;
        notification.ReadStatus = (int)ReadStatus.Read;

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
            await DeleteNotification(notification.NotificationId);
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
            ?? throw new NotFoundException(typeof(Notification));

        return notification.ToReponse();
    }

}
