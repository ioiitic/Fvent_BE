using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services
{
    public interface IEventResgistationService
    {
        Task<IdRes> RegisterFreeEvent(Guid id, Guid userId);
    }
}
