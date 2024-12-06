using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Request
{
    public record CreateNotificationReq(Guid userId, 
                                        Guid? eventId, 
                                        string title,
                                        string message);
    public record SendNotificationReq(string role,
                                      string title,
                                      string message);
}
