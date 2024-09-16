using Fvent.BO.Common;

namespace Fvent.Repository.Common;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetListAsync(ISpecification<TEntity> spec);
    Task<PageResult<TEntity>> GetPageAsync(ISpecification<TEntity> spec);
    Task<TEntity?> FindFirstOrDefaultAsync(ISpecification<TEntity> spec);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    void Update(TEntity existingEntity, TEntity updatedEntity);
    void Delete(TEntity entity);
}