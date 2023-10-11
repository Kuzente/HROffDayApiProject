using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstract
{
	public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
	{
		#region Ekleme İşlemleri
		Task<T> AddAsync(T entity);
		Task<List<T>> AddRangeAsync(List<T> entities);
		#endregion
		#region Güncelleme İşlemleri
		Task<T> Update(T entity);
		List<T> UpdateRange(List<T> entities);
		#endregion
		#region Kaydı durum değerini silindi olarak İşaretleme işlemleri
		Task<bool> DeleteByIdAsync(int Id);
		Task<bool> DeleteAsync(T entity);
		Task<bool> DeleteRangeAsync(List<T> entities);
		#endregion
		#region Kalıcı olarak silme işlemleri
		Task<bool> RemoveByIdAsync(int Id);
		Task<bool> RemoveAsync(T entity);
		Task<bool> RemoveRangeAsync(List<T> entities);
		#endregion
	}
}
