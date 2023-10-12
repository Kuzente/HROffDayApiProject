using Core.DTOs.BaseDTOs;
using Core.Interfaces;

namespace Services.Abstract
{
	public interface IWriteService<T,T1> : IService<T> where T : ReadBaseDto where T1 : WriteBaseDto
	{
		Task<IResultWithDataDto<T>> AddAsync(T1 writeDto);
		Task<IResultWithDataDto<T>> UpdateAsync(T1 writeDto);
		Task<bool> DeleteAsync(int id);
		Task<bool> RemoveAsync(int id);
	}
}
