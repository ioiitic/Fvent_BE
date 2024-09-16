using Fvent.BO.Entities;
using Fvent.Service.Request;
using Fvent.Service.Result;

namespace Fvent.Service.Mapper;

public static class CommonMapper
{
    public static IdRes ToResponse(
        this Guid id)
        => new(id);
}
