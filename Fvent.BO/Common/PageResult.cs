namespace Fvent.BO.Common;

public sealed record PageResult<TEntity>(IEnumerable<TEntity> Items, int PageNumber, int PageSize, int Count,
    int TotalItems, int TotalPages);
