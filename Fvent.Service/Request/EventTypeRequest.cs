using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Request
{
    public record CreateEventTypeReq(string eventTypeName);
    public record UpdateEventTypeReq(string eventTypeName);
}
