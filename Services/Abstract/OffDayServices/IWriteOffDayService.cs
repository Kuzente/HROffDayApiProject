using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.OffDayServices;

public interface IWriteOffDayService
{
    Task<bool> ChangeOffDayStatus(Guid id,bool isApproved);
    Task<IResultDto> AddOffDayService(WriteAddOffDayDto dto);
    Task<IResultDto> UpdateWaitingOffDayService(WriteUpdateWatingOffDayDto dto);

    Task<IResultDto> UpdateFirstWaitingStatusOffDayService(Guid id,bool status,string username);
    Task<IResultDto> UpdateSecondWaitingStatusOffDayService(Guid id,bool status,string username);
    //Task<IResultWithDataDto<>> UpdateAsync(T writeDto);
    Task<IResultDto> DeleteOffDayService(Guid id);
    // Task<IResultDto> RecoverAsync(Guid id);
    //Task<IResultDto> RemoveAsync(Guid id);
}