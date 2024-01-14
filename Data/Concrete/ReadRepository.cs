using Core.Entities;
using Data.Abstract;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Concrete;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
	private readonly DataContext _context;

	public ReadRepository(DataContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));
	public IQueryable<T> GetAll(bool disableTracking = true, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
	{
		IQueryable<T> query = _context.Set<T>();
		if (disableTracking)
			query = query.AsNoTracking();
		if (include != null)
			query = include(query);
		if (predicate != null)
			query = query.Where(predicate);
		if (orderBy != null)
			return orderBy(query);
		return query;
	}

	public async Task<bool> GetAny(Expression<Func<T, bool>>? predicate = null)
	{
		IQueryable<T> query = _context.Set<T>();
		query = query.AsNoTracking();
		return await Task.Run(() => query.Any(predicate));
	}

	public async Task<IQueryable<T>> GetByIdAsync(Guid id, bool disableTracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
	{
		IQueryable<T> query = _context.Set<T>();
		if (disableTracking)
			query = query.AsNoTracking();
		if (include != null)
			query = include(query);
		return await Task.Run((() => query.Where(d => d.ID == id)));
	}

	public async Task<T> GetSingleAsync(bool disableTracking = true, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
	{
		IQueryable<T> query = _context.Set<T>();
		if (disableTracking)
			query = query.AsNoTracking();
		if (include != null)
			query = include(query);
		if (predicate != null)
			query = query.Where(predicate);
		return await query.FirstOrDefaultAsync();
	}
}