using Fvent.Service.Request;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services;

public interface INotificationService
{
    Task<IList<NotificationRes>> GetListNotifications(Guid userId);
    Task<NotificationRes> GetNotification(Guid id);
    Task<IdRes> CreateNotification(CreateNotificationReq req);
    Task<IdRes> ReadNotification(Guid id);
    Task ClearNotification(Guid userId);
    Task DeleteNotification(Guid id);
}

