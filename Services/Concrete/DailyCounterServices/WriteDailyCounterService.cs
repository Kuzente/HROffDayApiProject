using Core;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.DailyCounterServices;

namespace Services.Concrete.DailyCounterServices;

public class WriteDailyCounterService : IWriteDailyCounterService
{
    private readonly IUnitOfWork _unitOfWork;

    public WriteDailyCounterService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResultDto> AddDailyYearCounterLogService()
    {
        IResultDto result = new ResultDto();
        try
        {
            var todayStartPersonals =  _unitOfWork.ReadPersonalRepository.GetAll(
                predicate: p =>
                    p.StartJobDate.Month == DateTime.UtcNow.AddHours(3).Month && 
                    p.StartJobDate.Day == DateTime.UtcNow.AddHours(3).Day &&
                    p.Status == EntityStatusEnum.Online).ToList();
            if (!todayStartPersonals.Any())
            {
                var log = new DailyYearLog
                {
                    NameSurname = "Personel Yok",
                    AddedYearLeave = 0,
                    AddedYearLeaveDescription = "Bugün işe giren personel yok!",
                    CreatedAt = DateTime.Now,
                }; 
                await _unitOfWork.WriteDailyYearLogRepository.AddAsync(log);
                var commit = _unitOfWork.Commit();
                if(!commit)
                    return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
                
            }
            else
            {
                var logList = new List<DailyYearLog>();
                todayStartPersonals.ForEach(a =>
                {
                    var log = new DailyYearLog();
                    int yearsSinceStart = (int)((DateTime.Now - a.StartJobDate).TotalDays / 365);
                    int yearsSinceBirth = (int)((DateTime.Now - a.BirthDate).TotalDays / 365);
                    switch (yearsSinceStart)
                    {
                        case >= 1 when (yearsSinceBirth >= 50 || yearsSinceBirth < 18 || a.RetiredOrOld):
                            a.TotalYearLeave += 20;
                            log.AddedYearLeave = 20;
                            log.AddedYearLeaveDescription =
                                yearsSinceBirth >= 50
                                    ? "Personel 50 yaşından büyük olduğu için 20 gün eklendi"
                                    : (yearsSinceBirth < 18
                                        ? "Personel 18 yaşından küçük olduğu için 20 gün eklendi"
                                        : "Personel Emekli olduğu için 20 gün eklendi");
                            break;
                        case >= 1 and <= 5:
                            a.TotalYearLeave += 14;
                            log.AddedYearLeave = 14;
                            log.AddedYearLeaveDescription =
                                $"{yearsSinceStart} yıl hizmet süresi olduğu için 14 gün eklendi";
                            break;
                        case > 5 and < 15:
                            a.TotalYearLeave += 20;
                            log.AddedYearLeave = 20;
                            log.AddedYearLeaveDescription =
                                $"{yearsSinceStart} yıl hizmet süresi olduğu için 20 gün eklendi";
                            break;
                        case >= 15:
                            a.TotalYearLeave += 26;
                            log.AddedYearLeave = 26;
                            log.AddedYearLeaveDescription =
                                $"{yearsSinceStart} yıl hizmet süresi olduğu için 26 gün eklendi";
                            break;
                        default:
                            log.AddedYearLeave = 0;
                            log.AddedYearLeaveDescription =
                                "Personel 1 seneyi doldurmadı. Ekleme Yapılmadı.";
                            break;
                    }

                    log.CreatedAt = DateTime.Now;
                    log.NameSurname = a.NameSurname;
                    logList.Add(log);
                });
                _unitOfWork.WritePersonalRepository.UpdateRange(todayStartPersonals);
                await _unitOfWork.WriteDailyYearLogRepository.AddRangeAsync(logList);
                var commit = _unitOfWork.Commit();
                if(!commit)
                    return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
            }
        }
        catch (Exception e)
        {
            result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }

    public async Task<IResultDto> AddDailyFoodAidCounterLogService()
    {
         IResultDto result = new ResultDto();
        try
        {
            var todayStartPersonals =  _unitOfWork.ReadPersonalRepository.GetAll(
                predicate: p =>
                    p.FoodAidDate.Month == DateTime.UtcNow.AddHours(3).Month && 
                    p.FoodAidDate.Day == DateTime.UtcNow.AddHours(3).Day &&
                    p.Status == EntityStatusEnum.Online).ToList();
            if (!todayStartPersonals.Any())
            {
                var log = new DailyFoodLog
                {
                    NameSurname = "Personel Yok",
                    AddedFoodAidAmount = 0,
                    AddedFoodAidAmountDescription = "Bugün işe giren personel yok!",
                    CreatedAt = DateTime.Now,
                }; 
                await _unitOfWork.WriteDailyFoodLogRepository.AddAsync(log);
                var commit = _unitOfWork.Commit();
                if(!commit)
                    return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
                
            }
            else
            {
                var logList = new List<DailyFoodLog>();
                todayStartPersonals.ForEach(a =>
                {
                    var log = new DailyFoodLog();
                    int yearsSinceStart = (int)((DateTime.Now - a.StartJobDate).TotalDays / 365);
                    switch (yearsSinceStart)
                    {
                        case 3:
                            a.FoodAid += 60;
                            log.AddedFoodAidAmount = 60;
                            log.AddedFoodAidAmountDescription = "3 yılı doldurduğu için 60 lira eklendi.";
                            break;
                        case > 3:
                            a.FoodAid += 20;
                            log.AddedFoodAidAmount = 20;
                            log.AddedFoodAidAmountDescription = "20 lira gıda yardımı eklendi.";
                            break;
                        default:
                            log.AddedFoodAidAmountDescription = "Gıda yardımı yapılmadı.";
                            break;
                    }
                    
                    log.CreatedAt = DateTime.Now;
                    log.NameSurname = a.NameSurname;
                    logList.Add(log);
                });
                _unitOfWork.WritePersonalRepository.UpdateRange(todayStartPersonals);
                await _unitOfWork.WriteDailyFoodLogRepository.AddRangeAsync(logList);
                var commit = _unitOfWork.Commit();
                if(!commit)
                    return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
            }
        }
        catch (Exception e)
        {
            result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }
}