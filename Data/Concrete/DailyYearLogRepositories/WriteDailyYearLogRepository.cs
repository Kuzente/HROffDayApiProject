using Core.Entities;
using Data.Abstract.DailyYearLogRepositories;
using Data.Context;

namespace Data.Concrete.DailyYearLogRepositories;

public class WriteDailyYearLogRepository : WriteRepository<DailyYearLog>, IWriteDailyYearLogRepository
{
    public WriteDailyYearLogRepository(DataContext context) : base(context)
    {
    }
}