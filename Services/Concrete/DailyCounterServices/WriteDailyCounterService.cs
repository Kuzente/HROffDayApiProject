using Core;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
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
                    p.YearLeaveDate.Month == DateTime.UtcNow.AddHours(3).Month && 
                    p.YearLeaveDate.Day == DateTime.UtcNow.AddHours(3).Day &&
                    p.Status == EntityStatusEnum.Online,
                include:i=> i.Include(p=>p.PersonalCumulatives)).ToList();
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
                    if (!a.PersonalCumulatives.Any(p=> p.Year == DateTime.UtcNow.AddHours(3).Year))
                    {
                        var personalCumulative = new PersonalCumulative
                        {
                            IsReportCompleted = false,
                            IsNotificationExist = false,
                            Year = DateTime.UtcNow.AddHours(3).Year
                        };
                        int yearsSinceStart = (int)((DateTime.UtcNow.AddHours(3) - a.YearLeaveDate).TotalDays / 365);
                        int yearsSinceBirth = (int)((DateTime.UtcNow.AddHours(3) - a.BirthDate).TotalDays / 365);
                        switch (yearsSinceStart)
                        {
                            case >= 1 when (yearsSinceBirth < 18 || a.IsYearLeaveRetired):
                                personalCumulative.EarnedYearLeave = 20;
                                personalCumulative.RemainYearLeave = 20;
                                a.TotalYearLeave += 20;
                                log.AddedYearLeave = 20;
                                log.AddedYearLeaveDescription =
                                    yearsSinceBirth < 18
                                        ? "Personel 18 yaşından küçük olduğu için 20 gün eklendi"
										: "Personel İyaş Bünyesinde Emekli olduğu için 20 gün eklendi";
                                break;
							case >= 1 when (yearsSinceBirth >= 50):
                                if (yearsSinceStart < 15)
                                {
									personalCumulative.EarnedYearLeave = 20;
									personalCumulative.RemainYearLeave = 20;
									a.TotalYearLeave += 20;
									log.AddedYearLeave = 20;
									log.AddedYearLeaveDescription = $"50 yaşından büyük ve {yearsSinceStart} yıl hizmet süresi olduğu için 20 gün eklendi";
								}
                                else
                                {
									personalCumulative.EarnedYearLeave = 26;
									personalCumulative.RemainYearLeave = 26;
									a.TotalYearLeave += 26;
									log.AddedYearLeave = 26;
									log.AddedYearLeaveDescription = $"50 yaşından büyük ve {yearsSinceStart} yıl hizmet süresi olduğu için 26 gün eklendi";
								}
								break;
							case >= 1 and <= 5:
                                a.TotalYearLeave += 14;
                                personalCumulative.EarnedYearLeave = 14;
                                personalCumulative.RemainYearLeave = 14;
                                log.AddedYearLeave = 14;
                                log.AddedYearLeaveDescription =
                                    $"{yearsSinceStart} yıl hizmet süresi olduğu için 14 gün eklendi";
                                break;
                            case > 5 and < 15:
                                a.TotalYearLeave += 20;
                                personalCumulative.EarnedYearLeave = 20;
                                personalCumulative.RemainYearLeave = 20;
                                log.AddedYearLeave = 20;
                                log.AddedYearLeaveDescription =
                                    $"{yearsSinceStart} yıl hizmet süresi olduğu için 20 gün eklendi";
                                break;
                            case >= 15:
                                a.TotalYearLeave += 26;
                                personalCumulative.EarnedYearLeave = 26;
                                personalCumulative.RemainYearLeave = 26;
                                log.AddedYearLeave = 26;
                                log.AddedYearLeaveDescription =
                                    $"{yearsSinceStart} yıl hizmet süresi olduğu için 26 gün eklendi";
                                break;
                            default:
                                log.AddedYearLeave = 0;
                                a.TotalYearLeave += 0;
                                personalCumulative.EarnedYearLeave = 0;
                                personalCumulative.RemainYearLeave = 0;
                                log.AddedYearLeaveDescription =
                                    "Personel 1 seneyi doldurmadı. Ekleme Yapılmadı.";
                                break;
                        }
                        a.PersonalCumulatives.Add(personalCumulative);
                    }
                    else
                    {
                        var currentYearCumulative = a.PersonalCumulatives.First(p => p.Year == DateTime.UtcNow.AddHours(3).Year && p.Status == EntityStatusEnum.Online);
                        log.AddedYearLeave = currentYearCumulative.EarnedYearLeave;
                        log.AddedYearLeaveDescription = "Personele ait yıllık izin manuel olarak eklendiği için otomasyonda tekrar yenilenmedi.";
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
            var todayStartPersonals =  await _unitOfWork.ReadPersonalRepository.GetAll(
                predicate: p =>
                    p.FoodAidDate.Month == DateTime.UtcNow.AddHours(3).Month && 
                    p.FoodAidDate.Day == DateTime.UtcNow.AddHours(3).Day &&
                    p.Status == EntityStatusEnum.Online).ToListAsync();
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
                var tasks = todayStartPersonals.Select(async a =>
                {
                    var log = new DailyFoodLog();
                    int yearsSinceStart = (int)((DateTime.UtcNow.AddHours(3) - a.FoodAidDate).TotalDays / 365);
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
                await Task.WhenAll(tasks);
				_unitOfWork.WritePersonalRepository.UpdateRange(todayStartPersonals);
                await _unitOfWork.WriteDailyFoodLogRepository.AddRangeAsync(logList);
                var commit = await Task.Run(()=> _unitOfWork.Commit());
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