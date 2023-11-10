using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;

namespace Services.Abstract.PersonalServices;

public interface IWritePersonalService 
{
    Task<IResultWithDataDto<AddPersonalDto>> AddAsync(AddPersonalDto writeDto);
    Task<bool> AddRangeAsync(List<AddRangePersonalDto> writeDto);
    Task<IResultWithDataDto<WritePersonalDto>> UpdateAsync(WritePersonalDto writeDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> RemoveAsync(int id);
    Task<bool> ChangeStatus(int id);
}