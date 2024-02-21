using Core.Entities;
using Core.Enums;
using Data.Abstract;

namespace Services.Hangfire;

public class DailyJob
{
    private readonly IUnitOfWork _unitOfWork;

    public DailyJob(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task YearLeaveEnhancer()
    {
        var todayStartPersonals =  _unitOfWork.ReadPersonalRepository.GetAll(
                predicate: p =>
            p.StartJobDate.Month == DateTime.Today.Month && 
            p.StartJobDate.Day == 18 &&
            p.Status == EntityStatusEnum.Online).ToList();
        if (!todayStartPersonals.Any())
        {
            var log = new DailyCounter
            {
                NameSurname = " ",
                AddedYearLeave = 0,
                AddedFoodAidAmount = 0,
                AddedYearLeaveDescription = "Bugün işe giren personel yok!",
                AddedFoodAidAmountDescription = "Bugün işe giren personel yok!",
                CreatedAt = DateTime.Now,
            }; 
            await _unitOfWork.WriteDailyCounterRepository.AddAsync(log);
            var commit = _unitOfWork.Commit();
            if(!commit)
                Console.WriteLine("Commit Olmadı");
                
        }
        else
        { 
            var logList = new List<DailyCounter>();
           todayStartPersonals.ForEach(a =>
           {
               var log = new DailyCounter();
               int yearsSinceStart = (int)((DateTime.Now - a.StartJobDate).TotalDays / 365);
               int yearsSinceBirth = (int)((DateTime.Now - a.BirthDate).TotalDays / 365);
               switch (yearsSinceStart)
               {
                   case 3:
                       log.AddedFoodAidAmount = 60;
                       log.AddedFoodAidAmountDescription = "3 yılı doldurduğu için 60 lira eklendi.";
                       break;
                   case > 3:
                       log.AddedFoodAidAmount = 20;
                       log.AddedFoodAidAmountDescription = "20 lira gıda yardımı eklendi.";
                       break;
                   default:
                       log.AddedFoodAidAmountDescription = "Gıda yardımı yapılmadı.";
                       break;
               }
               switch (yearsSinceStart)
               {
                   case >= 1 when (yearsSinceBirth >= 50 || yearsSinceBirth < 18 || a.RetiredOrOld):
                       a.TotalYearLeave += 20;
                       log.AddedYearLeave = 20;
                       log.AddedYearLeaveDescription =
                           yearsSinceBirth >= 50 ? "Personel 50 yaşından büyük olduğu için 20 gün eklendi" : (yearsSinceBirth < 18 ? "Personel 18 yaşından küçük olduğu için 20 gün eklendi" : "Personel Emekli olduğu için 20 gün eklendi");
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
           await _unitOfWork.WriteDailyCounterRepository.AddRangeAsync(logList);
           var commit = _unitOfWork.Commit();
           if(!commit)
               Console.WriteLine("Commit Başarısız");
        }
            
        
    }
    
}