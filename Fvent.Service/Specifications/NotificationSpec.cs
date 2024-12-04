using Fvent.BO.Entities;
using Fvent.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Specifications;
public static class NotificationSpec
{
    public class GetNotificationSpec : Specification<Notification>
    {
        public GetNotificationSpec()
        {
            Include(u => u.User!);
            Include(u => u.Event!);
        }
        public GetNotificationSpec(Guid id)
        {
            Filter(u => u.NotificationId == id);

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }

    public class GetNotificationByUserSpec : Specification<Notification>
    {
        public GetNotificationByUserSpec(Guid id)
        {
            Filter(u => u.UserId == id);
            OrderBy(u => u.SentTime, true);

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }

    public class GetUnreadNotificationByUserSpec : Specification<Notification>
    {
        public GetUnreadNotificationByUserSpec(Guid id)
        {
            Filter(u => u.UserId == id && u.ReadStatus == ReadStatus.Unread);

            Include(u => u.User!);
            Include(u => u.Event!);
        }
    }
}
