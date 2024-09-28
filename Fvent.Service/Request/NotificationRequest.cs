﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Request
{
    public record CreateNotificationReq(Guid userId, 
                                        Guid eventId, 
                                        string message);
}
