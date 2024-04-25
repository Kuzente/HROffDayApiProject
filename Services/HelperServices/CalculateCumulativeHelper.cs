using Core.Entities;

namespace Services.HelperServices;

public static class CalculateCumulativeHelper
{
    public static string CalculateCumulative(DateTime yearLeaveDate,DateTime birthDate,bool isYearLeaveRetired,DateTime? retiredDate)
    {
        int calisilanYil = 0;
        string cumulativeString = "";
        DateTime oldControlYear = new DateTime(2003, 5, 10);
        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        for (int year = yearLeaveDate.Year; year <= DateTime.Now.Year; year++)
        {
            var iseBaslamaKontrolDate = new DateTime(year, yearLeaveDate.Month, yearLeaveDate.Day);
            if (iseBaslamaKontrolDate <= todayDate)
            {
                var age = CalculateAge(iseBaslamaKontrolDate, birthDate);
                if (calisilanYil == 0 && iseBaslamaKontrolDate <= todayDate)
                {
                    cumulativeString += "0+";
                }
                else if (age is >= 50 or < 18 || (isYearLeaveRetired && retiredDate.HasValue && iseBaslamaKontrolDate > retiredDate))
                {
                    if (iseBaslamaKontrolDate < oldControlYear)
                    {
                        cumulativeString += "12+";
                    }
                    else
                    {
                        cumulativeString += "20+";
                    } 
                }
                else
                {
                    if (calisilanYil <= 5) {
                        if (iseBaslamaKontrolDate < oldControlYear){
                            cumulativeString += "12+";
                        }
                        else{
                            cumulativeString += "14+";
                        }
                    } else if (calisilanYil < 15) {
                        if (iseBaslamaKontrolDate < oldControlYear){
                            cumulativeString += "18+";
                        }
                        else{
                            cumulativeString += "20+";
                        }
                        
                    }
                    else
                    {
                        if (iseBaslamaKontrolDate < oldControlYear)
                        {
                            cumulativeString += "24+";
                        }
                        else
                        {
                            cumulativeString += "26+";
                        }
                    }
                }
                calisilanYil++;
            }
            else
            {
                break;
            }
            
        }

        return cumulativeString;
    }

    private static int CalculateAge(DateTime dateToBeCalculated, DateTime dogumTarihi)
    {
        int age = dateToBeCalculated.Year - dogumTarihi.Year;
        if (dateToBeCalculated.Month < dogumTarihi.Month)
        {
            age--;
        }
        else if (dateToBeCalculated.Month == dogumTarihi.Month && dateToBeCalculated.Day < dogumTarihi.Day)
        {
            age--;
        }
        return age;
    }

    public static List<PersonalCumulative> GetCumulativeList(string cumulativeFormula,int yearLeaveDateYear,int usedYearLeave)
    {
        cumulativeFormula = cumulativeFormula.TrimEnd('+');
        var listOfGainLeave = cumulativeFormula.Split('+').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();
        var personalCumulative = new List<PersonalCumulative>();
        int year = yearLeaveDateYear;
        foreach (var gainLeave in listOfGainLeave)
        {
            int remainYearLeave = gainLeave;
            if (gainLeave <= usedYearLeave) {
                usedYearLeave -= gainLeave;
                remainYearLeave = 0;
            } else {
                remainYearLeave -= usedYearLeave;
                usedYearLeave = 0;
            }
            //int remainYearLeave = Math.Max(gainLeave - usedYearLeave, 0);
            personalCumulative.Add(new PersonalCumulative
            {
                Year = year,
                EarnedYearLeave = gainLeave,
                RemainYearLeave = remainYearLeave,
                IsReportCompleted = remainYearLeave == 0
                
            });
            year++;
        }
        var lastZeroLeave = personalCumulative
            .OrderByDescending(p => p.Year)
            .FirstOrDefault(p => p.RemainYearLeave == 0);
        if (lastZeroLeave != null && lastZeroLeave.Year != yearLeaveDateYear)
        {
            lastZeroLeave.IsNotificationExist = true;
        }
        return personalCumulative;
    }
    
}