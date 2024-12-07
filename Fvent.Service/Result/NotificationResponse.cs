using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Result
{
    public record NotificationRes(Guid notiId,
                                  Guid userId,
                                  Guid? eventId,
                                  string title,
                                  string message,
                                  string readStatus,
                                  DateTime sendTime);
}
