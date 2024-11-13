using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
using System.Text.Json;
using static Fvent.Service.Specifications.FormSpec;

namespace Fvent.Service.Services.Imp;

public class FormService(IUnitOfWork uOW, IRegistationService registationService) : IFormService
{
    public async Task<IList<FormSubmitRes>> GetFormSubmits(Guid eventId)
    {
        var spec = new GetFormSubmitSpec(eventId);
        var res = await uOW.FormSubmit.GetListAsync(spec);
        //var test2 = JsonSerializer.Deserialize<Object>(src.Data);
        var test = res.Select(f => f.ToResponse()).ToList();

        return res.Select(f => f.ToResponse()).ToList();
    }

    public async Task<IdRes> SubmitForm(Guid eventId, Guid userId, FormSubmitReq req)
    {
        var formSubmit = req.ToSubmit(eventId, userId);
        await uOW.FormSubmit.AddAsync(formSubmit);
        await uOW.SaveChangesAsync();

        await registationService.RegisterFreeEvent(eventId, userId);

        return formSubmit.FormSubmitId.ToResponse();
    }

    public async Task<IList<FormSubmitRes>> GetFormSubmits(Guid eventId, Guid userId)
    {
        var spec = new GetFormSubmitSpec(eventId, userId);
        var res = await uOW.FormSubmit.GetListAsync(spec);

        return res.Select(f => f.ToResponse()).ToList();
    }
}
