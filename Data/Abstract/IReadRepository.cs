using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstract
{
	public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
	{
		IQueryable<T> GetAll(bool disableTracking = true, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
		Task<T> GetSingleAsync(bool disableTracking = true, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
		Task<T> GetByIdAsync(int Id, bool disableTracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
		bool GetAny(Expression<Func<T, bool>>? predicate = null);
	}
}
