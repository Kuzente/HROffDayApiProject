using Core.DTOs.OffDayDTOs;

namespace Services.Abstract.OffDayServices;

public interface IWriteOffDayService : IWriteService<ReadOffDayDto,WriteOffDayDto>
{
    Task<bool> ChangeOffDayStatus(int id,bool isApproved);
}