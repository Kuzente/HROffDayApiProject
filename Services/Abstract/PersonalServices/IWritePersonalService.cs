using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalCumulativeDtos.WriteDtos;
using Core.DTOs.PersonalDetailDto.WriteDtos;
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
    Task<IResultDto> ChangeStatus(WritePersonalChangeStatusDto dto);
    Task<IResultDto> RecoverAsync(Guid id);
    Task<IResultDto> UpdatePersonalCumulativeAsyncService(WriteUpdateCumulativeDto dto);
    Task<IResultDto> UpdatePersonalCumulativeNotificationAsyncService(Guid id);
    Task<IResultDto> DeletePersonalCumulativeAsyncService(Guid personalId,Guid cumulativeId);
}