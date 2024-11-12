using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Services;

public interface IFormService
{
    Task<IList<FormSubmitRes>> GetFormSubmits(Guid eventId);
    Task<IdRes> SubmitForm(Guid eventId, Guid userId, FormSubmitReq req);
    Task<IList<FormSubmitRes>> GetFormSubmits(Guid eventId, Guid userId);
}
