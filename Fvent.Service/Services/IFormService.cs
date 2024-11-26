using Fvent.Service.Request;
using Fvent.Service.Result;
using Microsoft.Extensions.Logging;

namespace Fvent.Service.Services;

public interface IFormService
{
    Task<IList<FormSubmitRes>> GetFormSubmits(Guid eventId);
    Task<IdRes> SubmitForm(Guid eventId, Guid userId, FormSubmitReq req);
    Task<FormSubmitRes> GetFormSubmit(Guid eventId, Guid userId);
    Task<bool> DeleteFormSubmit(Guid eventId, Guid userId); 
}
