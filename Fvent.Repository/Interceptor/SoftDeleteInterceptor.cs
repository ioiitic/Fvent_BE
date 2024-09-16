using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Fvent.BO.Common;

namespace Fvent.Repository.Interceptor;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null) return new ValueTask<InterceptionResult<int>>(result);

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;

            entry.State = EntityState.Modified;

            delete.IsDeleted = true;
            delete.DeletedAt = DateTime.UtcNow;
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}