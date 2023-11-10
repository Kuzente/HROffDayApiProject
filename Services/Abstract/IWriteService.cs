using Core.DTOs.BaseDTOs;
using Core.Interfaces;

namespace Services.Abstract;

public interface IWriteService<T> : IService<T> where T : BaseDto
{
	Task<IResultWithDataDto<T>> AddAsync(T writeDto);
	Task<IResultWithDataDto<T>> UpdateAsync(T writeDto);
	Task<IResultDto> DeleteAsync(int id);
	Task<IResultDto> RecoverAsync(int id);
	Task<IResultDto> RemoveAsync(int id);
}