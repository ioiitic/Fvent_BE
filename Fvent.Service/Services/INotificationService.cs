using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface INotificationService
{
    Task<NotificationRes> GetNotification(Guid id);
    Task<IList<NotificationRes>> GetListNotifications(Guid userId);
    Task<IdRes> CreateNotification(CreateNotificationReq req);
    Task<IdRes> ReadNotification(Guid id);
    Task ClearNotification(Guid userId);
    Task SendNotification(SendNotificationReq req);
    Task DeleteNotification(Guid id);
}

