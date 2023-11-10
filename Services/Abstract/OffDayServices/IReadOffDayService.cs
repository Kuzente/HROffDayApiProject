using Core.DTOs.OffDayDTOs;

namespace Services.Abstract.OffDayServices;

public interface IReadOffDayService
{
    Task<List<ReadOffDayDto>> GetAllWithPersonal();
    
}