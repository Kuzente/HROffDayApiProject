using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.PersonalServices;

public interface IWritePersonalService 
{
    Task<IResultDto> AddAsync(AddPersonalDto writeDto);
    Task<IResultDto> AddRangeAsync(List<AddRangePersonalDto> writeDto);
    Task<IResultDto> UpdateAsync(WriteUpdatePersonalDto writeDto);
    Task<IResultDto> DeleteAsync(Guid id);
    Task<bool> RemoveAsync(Guid id);
    Task<IResultDto> ChangeStatus(Guid id);
    Task<IResultDto> RecoverAsync(Guid id);
}