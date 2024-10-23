using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IRegistationService
{
    Task<IdRes> RegisterFreeEvent(Guid id, Guid userId);
    Task UnRegisterEvent(Guid eventId, Guid userId);
    Task<IList<UserRes>> GetAllParticipantsForEvent(Guid eventId);
}
