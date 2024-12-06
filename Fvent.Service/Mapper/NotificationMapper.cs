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
            return new Notification(
                src.userId,
                src.eventId ,
                src.title,
                src.message,
                ReadStatus.Unread
            );
        }

        public static Notification ToNotification(this SendNotificationReq src, Guid userId)
        {
            return new Notification(
                userId,
                null,
                src.title,
                src.message,
                ReadStatus.Unread
            );
        }


        public static NotificationRes ToReponse(this Notification src)
        {
            return new NotificationRes(
                src.NotificationId,
                src.UserId,
                src.EventId,
                src.Title,
                src.Message,
                src.ReadStatus.ToString(),
                src.SentTime
            );
        }
    }
}
