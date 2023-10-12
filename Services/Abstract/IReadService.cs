using Core.DTOs.BaseDTOs;

namespace Services.Abstract
{
	public interface IReadService<T> : IService<T> where T : BaseDto
	{
		Task<List<T>> GetAllAsync();
		Task<T> GetSingleAsync();
		Task<bool> GetAnyAsync();
	}
}
