using Fvent.Repository.Repositories;

namespace Fvent.Repository.UOW;

public interface IUnitOfWork
{
    IEventRepo Events { get; }
    IEventFollowerRepo EventFollower { get; }
    IReviewRepo Reviews { get; }
    IUserRepo Users { get; }

    bool IsUpdate<TEntity>(TEntity entity) where TEntity : class;
    Task SaveChangesAsync();
    void SaveChanges();
}
