using Core.Entities;
using Data.Abstract;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Concrete
{
	public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
	{
		private readonly DataContext _context;

		public WriteRepository(DataContext context) => _context = context ?? throw new ArgumentNullException(nameof(DataContext));
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
				_ = await Task.Run(() => _context.Update(entity));
				return true;
			}
			catch (DbUpdateException ex)
			{
				throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
			}
		}

		public async Task<bool> DeleteByIdAsync(int Id)
		{
			try
			{ 
				var entity = await _context.FindAsync<T>(Id);
				if (entity != null)
				{
					entity.Status = Core.Enums.EntityStatusEnum.Archive;
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
					await Task.Run(() => entity.Status = Core.Enums.EntityStatusEnum.Archive);
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
				_ = await Task.Run(() => _context.Remove<T>(entity));
				return true;
			}
			catch (DbUpdateException ex)
			{
				throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
			}
		}

		public async Task<bool> RemoveByIdAsync(int Id)
		{
			try
			{
				var entity = await _context.FindAsync<T>(Id);
				if (entity == null)
					return false;
				_ = await Task.Run(() => _context.Remove<T>(entity));
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

		public async Task<T> Update(T entity)
		{
			try
			{
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
}
