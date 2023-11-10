using Core.Entities;
using Data.Abstract;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Concrete;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
	private readonly DataContext _context;

	public WriteRepository(DataContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));
	public async Task<T> AddAsync(T entity)
	{
		try
		{
			_ = await _context.AddAsync(entity);				
			return entity;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<List<T>> AddRangeAsync(List<T> entities)
	{
		try
		{
			await _context.AddRangeAsync(entities);
			return entities;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> DeleteAsync(T entity)
	{
		try
		{
			entity.Status = Core.Enums.EntityStatusEnum.Archive;
			entity.DeletedAt = DateTime.Now;
			_ = await Task.Run(() => _context.Update(entity));
			return true;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> DeleteByIdAsync(int id)
	{
		try
		{ 
			var entity = await _context.FindAsync<T>(id);
			if (entity != null)
			{
				entity.Status = Core.Enums.EntityStatusEnum.Archive;
				entity.DeletedAt = DateTime.Now;
				_ = await Task.Run(()=> _context.Update(entity));
				return true;
			}
			return false;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> DeleteRangeAsync(List<T> entities)
	{
		try
		{
			await Task.Run(() => _context.UpdateRange(entities));
			foreach (var entity in entities)
				await Task.Run(() => entity.Status = Core.Enums.EntityStatusEnum.Archive );
			return true;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> RemoveAsync(T entity)
	{
		try
		{
			_ = await Task.Run(() => _context.Remove(entity));
			return true;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> RemoveByIdAsync(int id)
	{
		try
		{
			var entity = await _context.FindAsync<T>(id);
			if (entity == null)
				return false;
			_ = await Task.Run(() => _context.Remove(entity));
			return true;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> RemoveRangeAsync(List<T> entities)
	{
		try
		{
			await Task.Run(() => _context.RemoveRange(entities));
			return true;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<bool> RecoverAsync(int id)
	{
		try
		{ 
			var entity = await _context.FindAsync<T>(id);
			if (entity != null)
			{
				entity.Status = Core.Enums.EntityStatusEnum.Online;
				entity.DeletedAt = DateTime.MinValue;
				entity.ModifiedAt = DateTime.Now;
				_ = await Task.Run(()=> _context.Update(entity));
				return true;
			}
			return false;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public async Task<T> Update(T entity)
	{
		try
		{
			entity.ModifiedAt = DateTime.Now;
			_ = await Task.Run(()=> _context.Update(entity));
			return entity;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}

	public List<T> UpdateRange(List<T> entities)
	{
		try
		{
			_context.UpdateRange(entities);
			return entities;
		}
		catch (DbUpdateException ex)
		{
			throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
		}
	}
}