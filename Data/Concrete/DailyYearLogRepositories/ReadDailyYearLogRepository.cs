using Core.Entities;
using Data.Abstract.DailyYearLogRepositories;
using Data.Context;

namespace Data.Concrete.DailyYearLogRepositories;

public class ReadDailyYearLogRepository : ReadRepository<DailyYearLog>, IReadDailyYearLogRepository
{
    public ReadDailyYearLogRepository(DataContext context) : base(context)
    {
    }
}