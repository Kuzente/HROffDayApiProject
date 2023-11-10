using Core.Entities;

namespace Data.Abstract;

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
	Task<bool> DeleteByIdAsync(int id);
	Task<bool> DeleteAsync(T entity);
	Task<bool> DeleteRangeAsync(List<T> entities);
	#endregion
	#region Kalıcı olarak silme işlemleri
	Task<bool> RemoveByIdAsync(int id);
	Task<bool> RemoveAsync(T entity);
	Task<bool> RemoveRangeAsync(List<T> entities);
	#endregion

	#region Silinen değeri geri döndürme İşlemi

	Task<bool> RecoverAsync(int id);

	#endregion
}