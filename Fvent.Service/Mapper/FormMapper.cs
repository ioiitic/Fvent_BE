using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;
using System.Text.Json;

namespace Fvent.Service.Mapper;

public static class FormMapper
{
    public static FormSubmitRes ToResponse(this FormSubmit src)
        => new(JsonSerializer.Deserialize<Object>(src.Data)!, src.User!.ToResponse<UserRes>());

    public static FormSubmit ToSubmit(this FormSubmitReq src, Guid eventId, Guid userId)
        => new(userId, eventId, src.Data.ToString()!);
}
