using Fvent.BO.Entities;
using Fvent.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Specifications
{
    public static class EventTypeSpec
    {
        public class GetEventTypeSpec : Specification<EventType>
        {
            public GetEventTypeSpec(Guid eventTypeId)
            {
                Filter(u => u.EventTypeId == eventTypeId);
            }
            public GetEventTypeSpec()
            {
            }
        }
    }
}
