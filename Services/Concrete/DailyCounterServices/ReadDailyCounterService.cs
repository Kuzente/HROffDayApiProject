using AutoMapper;
using Core.DTOs;
using Core.DTOs.DailyCounterDto;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.DailyCounterServices;

namespace Services.Concrete.DailyCounterServices;

public class ReadDailyCounterService : IReadDailyCounterService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReadDailyCounterService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResultWithDataDto<List<TodayStartPersonalYearDto>>> GetLastHundredDailyYearLogService()
    {
        IResultWithDataDto<List<TodayStartPersonalYearDto>> res = new ResultWithDataDto<List<TodayStartPersonalYearDto>>();
        try
        {
            var resultData = _unitOfWork.ReadDailyYearLogRepository.GetAll(
                orderBy: p => p.OrderByDescending(a => a.CreatedAt)).Take(100).ToList();
            var mappedResult = _mapper.Map<List<TodayStartPersonalYearDto>>(resultData);
            res.SetData(mappedResult);
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

    public async Task<IResultWithDataDto<List<TodayStartPersonalFoodDto>>> GetLastHundredDailyFoodLogService()
    {
        IResultWithDataDto<List<TodayStartPersonalFoodDto>> res = new ResultWithDataDto<List<TodayStartPersonalFoodDto>>();
        try
        {
            var resultData = _unitOfWork.ReadDailyFoodLogRepository.GetAll(
                orderBy: p => p.OrderByDescending(a => a.CreatedAt)).Take(100).ToList();
            
            var mappedResult = _mapper.Map<List<TodayStartPersonalFoodDto>>(resultData);
            res.SetData(mappedResult);
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }
}