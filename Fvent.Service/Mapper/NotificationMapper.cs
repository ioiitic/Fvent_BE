using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Service.Request;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Mapper
{
    public static class NotificationMapper
    {
        public static Notification ToNotification(this CreateNotificationReq src)
        {
            return Notification.Create(
                src.userId,
                src.eventId,
                src.message,
                ReadStatus.Unread
            );
        }


        public static NotificationRes ToReponse(this Notification src)
        {
            return new NotificationRes(
                src.EventId,
                src.UserId,
                src.Message,
                ((ReadStatus)src.ReadStatus).ToString(), // Convert the enum to its string name
                src.SentTime
            );
        }
    }
}
