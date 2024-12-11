using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Request;
using Fvent.Service.Result;
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
        var user = await uOW.Users.GetByIdAsync(userId);
        if (user.IsBanned)
        {
            throw new Exception("Your account has been banned");
        }
        var formSubmit = req.ToSubmit(eventId, userId);
        await uOW.FormSubmit.AddAsync(formSubmit);
        await uOW.SaveChangesAsync();

        await registationService.RegisterFreeEvent(eventId, userId);

        return formSubmit.FormSubmitId.ToResponse();
    }

    public async Task<FormSubmitRes> GetFormSubmit(Guid eventId, Guid userId)
    {
        var spec = new GetFormSubmitSpec(eventId, userId);
        var res = await uOW.FormSubmit.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(FormSubmit));

        return res.ToResponse();
    }

    public async Task<bool> DeleteFormSubmit(Guid eventId, Guid userId)
    {
        var spec = new GetFormSubmitSpec(eventId, userId);
        var res = await uOW.FormSubmit.FindFirstOrDefaultAsync(spec)
            ?? throw new NotFoundException(typeof(FormSubmit));

        uOW.FormSubmit.Delete(res);
        await uOW.SaveChangesAsync();

        return true;
    }
}
