using Microsoft.EntityFrameworkCore;
using Fvent.BO.Common;
using Fvent.Repository.Data;
using static System.Math;

namespace Fvent.Repository.Common;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly MyDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(MyDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

    public Task<IEnumerable<TEntity>> GetListAsync(ISpecification<TEntity> spec)
    {
        IQueryable<TEntity> query = Query(spec);

        return Task.FromResult<IEnumerable<TEntity>>(query);
    }

    public async Task<PageResult<TEntity>> GetPageAsync(ISpecification<TEntity> spec)
    {
        IQueryable<TEntity> query = Query(spec);

        // Apply pagination before retrieving items
        if (spec.PageNumber > 0 && spec.PageSize > 0)
        {
            var skip = (spec.PageNumber - 1) * spec.PageSize;
            query = query.Skip(skip).Take(spec.PageSize);
        }

        // Get paginated items
        var items = await query.ToListAsync();

        // Calculate total items without pagination
        var totalItems = await Query(spec).CountAsync();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalItems / (double)spec.PageSize);

        return new PageResult<TEntity>(items, spec.PageNumber, spec.PageSize, items.Count, totalItems, totalPages);
    }


    public async Task<TEntity?> FindFirstOrDefaultAsync(ISpecification<TEntity> spec)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        if (spec is null)
        {
            throw new Exception("Null specification");
        }

        if (spec != null)
        {
            if (spec.IgnoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (spec.Filters is not null)
                query = query.Where(spec.Filters);

            query = spec.Includes.Aggregate(query,
                (current, include) => current.Include(include));

            query = spec.IncludeStrings.Aggregate(query,
                (current, includeString) => current.Include(includeString));
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Update(TEntity existingEntity, TEntity updatedEntity)
    {
        _dbSet.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    private IQueryable<TEntity> Query(ISpecification<TEntity> spec)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        if (spec is null)
        {
            throw new Exception("Null specification");
        }

        if (spec.Selections is not null)
            query = query.Select(spec.Selections);

        if (spec.Filters is not null)
            query = query.Where(spec.Filters);

        query = spec.Includes.Aggregate(query,
            (current, include) => current.Include(include));

        query = spec.IncludeStrings.Aggregate(query,
            (current, includeString) => current.Include(includeString));

        if (spec.OrderByAscending != null)
        {
            query = query.OrderBy(spec.OrderByAscending);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        query = spec.ThenBys.Aggregate(query,
            (query, thenBy) => thenBy.Item2
                ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(thenBy.Item1)
                : ((IOrderedQueryable<TEntity>)query).ThenBy(thenBy.Item1));

        return query;
    }
}
