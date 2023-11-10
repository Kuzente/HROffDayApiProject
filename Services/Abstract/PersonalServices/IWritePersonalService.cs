using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;

namespace Services.Abstract.PersonalServices;

public interface IWritePersonalService 
{
    Task<IResultDto> AddAsync(AddPersonalDto writeDto);
    Task<IResultDto> AddRangeAsync(List<AddRangePersonalDto> writeDto);
    Task<IResultWithDataDto<WritePersonalDto>> UpdateAsync(WritePersonalDto writeDto);
    Task<IResultDto> DeleteAsync(int id);
    Task<bool> RemoveAsync(int id);
    Task<IResultDto> ChangeStatus(int id);
    Task<IResultDto> RecoverAsync(int id);
}