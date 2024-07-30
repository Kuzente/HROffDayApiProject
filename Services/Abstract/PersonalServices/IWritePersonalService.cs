using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalCumulativeDtos.WriteDtos;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.PersonalServices;

public interface IWritePersonalService 
{
    Task<IResultDto> AddAsync(AddPersonalDto writeDto,Guid userId,string ipAddress);
    Task<IResultDto> AddRangeAsync(List<AddRangePersonalDto> writeDto,Guid userId,string ipAddress);
    Task<IResultDto> UpdateAsync(WriteUpdatePersonalDto writeDto,Guid userId,string ipAddress);
    Task<IResultDto> DeleteAsync(Guid id,Guid userId,string ipAddress);
    Task<IResultDto> ChangeStatus(WritePersonalChangeStatusDto dto,Guid userId,string ipAddress);
    Task<IResultDto> RecoverAsync(Guid id,Guid userId,string ipAddress);
    Task<IResultDto> UpdatePersonalCumulativeAsyncService(WriteUpdateCumulativeDto dto,Guid userId,string ipAddress);
    Task<IResultDto> UpdatePersonalCumulativeNotificationAsyncService(Guid id,Guid userId,string ipAddress);
}