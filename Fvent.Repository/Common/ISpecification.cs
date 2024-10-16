using System.Linq.Expressions;

namespace Fvent.Repository.Common;

public interface ISpecification<TEntity> where TEntity : class
{
    internal IEnumerable<Expression<Func<TEntity, object>>> Includes { get; }
    internal IEnumerable<string> IncludeStrings { get; }
    internal Expression<Func<TEntity, object>>? OrderByAscending { get; }
    internal Expression<Func<TEntity, object>>? OrderByDescending { get; }
    internal IEnumerable<Tuple<Expression<Func<TEntity, object>>, bool>> ThenBys { get; }
    internal Expression<Func<TEntity, TEntity>>? Selections { get; }
    internal Expression<Func<TEntity, bool>>? Filters { get; }
    internal int PageNumber { get; }
    internal int PageSize { get; }
    bool IgnoreQueryFilters { get; }
    ISpecification<TEntity> Select(Expression<Func<TEntity, TEntity>> select);
    ISpecification<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
    ISpecification<TEntity> Include(Expression<Func<TEntity, object>> include);
    ISpecification<TEntity> Include(string includeString);
    ISpecification<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy, bool isDescending);
    ISpecification<TEntity> ThenBy(Expression<Func<TEntity, object>> thenBy, bool isDescending);
    ISpecification<TEntity> AddPagination(int pageNumber, int pageSize);
}
