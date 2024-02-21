using Core.DTOs.DailyCounterDto;
using Core.Interfaces;

namespace Services.Abstract.DailyCounterServices;

public interface IReadDailyCounterService
{
    Task<IResultWithDataDto<List<TodayStartPersonalDto>>> GetLastHundredLogService(); // Son Eklenen 100 Logu getir
}