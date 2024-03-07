using Core.DTOs.DailyCounterDto;
using Core.Interfaces;

namespace Services.Abstract.DailyCounterServices;

public interface IReadDailyCounterService
{
    Task<IResultWithDataDto<List<TodayStartPersonalYearDto>>> GetLastHundredDailyYearLogService(); // Son Eklenen 100 Yıllık İzin Logu getir
    Task<IResultWithDataDto<List<TodayStartPersonalFoodDto>>> GetLastHundredDailyFoodLogService(); // Son Eklenen 100 Yemek Yardımı Logu getir
}