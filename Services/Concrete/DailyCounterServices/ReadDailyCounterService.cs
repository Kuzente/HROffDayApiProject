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

    public async Task<IResultWithDataDto<List<TodayStartPersonalDto>>> GetLastHundredLogService()
    {
        IResultWithDataDto<List<TodayStartPersonalDto>> res = new ResultWithDataDto<List<TodayStartPersonalDto>>();
        try
        {
            var resultData = _unitOfWork.ReadDailyCounterRepository.GetAll(
                orderBy: p => p.OrderByDescending(a => a.CreatedAt)).Take(100).ToList();
            var mappedResult = _mapper.Map<List<TodayStartPersonalDto>>(resultData);
            res.SetData(mappedResult);
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }
}