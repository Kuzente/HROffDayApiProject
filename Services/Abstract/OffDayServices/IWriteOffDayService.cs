using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.OffDayServices;

public interface IWriteOffDayService
{
    Task<IResultDto> AddOffDayService(WriteAddOffDayDto dto,Guid userId,string ipAddress);
    Task<IResultDto> UpdateWaitingOffDayService(WriteUpdateWatingOffDayDto dto,Guid userId,string ipAddress);
    Task<IResultDto> UpdateApprovedOffDayService(WriteUpdateWatingOffDayDto dto,Guid userId,string ipAddress);

    Task<IResultDto> UpdateFirstWaitingStatusOffDayService(Guid id,bool status,string username,Guid userId,string ipAddress);
    Task<IResultDto> UpdateSecondWaitingStatusOffDayService(Guid id,bool status,string username,Guid userId,string ipAddress);
    Task<IResultDto> DeleteOffDayService(Guid id,Guid userId,string ipAddress);
    
}