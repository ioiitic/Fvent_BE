using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;

public static class FormMapper
{
    public static FormSubmitRes ToResponse(this FormSubmit src)
        => new(src.Data, src.User!.ToResponse<UserRes>());

    public static FormSubmit ToSubmit(this FormSubmitReq src, Guid eventId, Guid userId)
        => new(userId, eventId, src.Data);
}
