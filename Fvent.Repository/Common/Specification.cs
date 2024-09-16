using LinqKit;
using System.Linq.Expressions;

namespace Fvent.Repository.Common;

public class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    private readonly List<Expression<Func<TEntity, bool>>> _filters = [];
    private Expression<Func<TEntity, TEntity>>? _selects;
    private readonly List<Expression<Func<TEntity, object>>> _includes = [];
    private readonly List<string> _includeStrings = [];
    private Expression<Func<TEntity, object>>? _orderByAscending;
    private Expression<Func<TEntity, object>>? _orderByDescending;
    private readonly List<Tuple<Expression<Func<TEntity, object>>, bool>> _thenBys = [];
    private int _pageNumber = 0;
    private int _pageSize = 0;

    public ISpecification<TEntity> Select(Expression<Func<TEntity, TEntity>> select)
    {
        _selects = select;
        return this;
    }

    public ISpecification<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
    {
        _filters.Add(filter);
        return this;
    }

    public ISpecification<TEntity> Include(Expression<Func<TEntity, object>> include)
    {
        _includes.Add(include);
        return this;
    }

    public ISpecification<TEntity> Include(string includeString)
    {
        _includeStrings.Add(includeString);
        return this;
    }

    public ISpecification<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy, bool isDescending)
    {
        _orderByAscending = null;
        _orderByDescending = null;

        if (orderBy != null)
        {
            if (!isDescending)
            {
                _orderByAscending = orderBy;
            }
            else
            {
                _orderByDescending = orderBy;
            }
        }

        return this;
    }

    public ISpecification<TEntity> ThenBy(Expression<Func<TEntity, object>> thenBy, bool isDescending)
    {
        _thenBys.Add(Tuple.Create(thenBy, isDescending));
        return this;
    }

    public ISpecification<TEntity> AddPagination(int pageNumber, int pageSize)
    {
        _pageNumber = pageNumber;
        _pageSize = pageSize;
        return this;
    }

    public IEnumerable<Expression<Func<TEntity, object>>> Includes => _includes;
    public IEnumerable<string> IncludeStrings => _includeStrings;
    public Expression<Func<TEntity, object>>? OrderByAscending => _orderByAscending;
    public Expression<Func<TEntity, object>>? OrderByDescending => _orderByDescending;
    public Expression<Func<TEntity, TEntity>>? Selections => _selects;
    public Expression<Func<TEntity, bool>>? Filters => Specification<TEntity>.ToExpression(_filters);
    public IEnumerable<Tuple<Expression<Func<TEntity, object>>, bool>> ThenBys => _thenBys;
    public int PageNumber => _pageNumber;
    public int PageSize => _pageSize;

    private static Expression<Func<TEntity, bool>>? ToExpression(List<Expression<Func<TEntity, bool>>> list)
    {
        if (list.Count == 0)
        {
            return null;
        }

        var predicate = PredicateBuilder.New<TEntity>(true);

        list.ForEach(filter =>
        {
            predicate = predicate.And(filter);
        });

        return predicate;
    }
}
