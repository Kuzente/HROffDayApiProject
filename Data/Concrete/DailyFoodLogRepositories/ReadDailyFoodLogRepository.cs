using Core.Entities;
using Data.Abstract.DailyFoodLogRepositories;
using Data.Context;

namespace Data.Concrete.DailyFoodLogRepositories;

public class ReadDailyFoodLogRepository : ReadRepository<DailyFoodLog>, IReadDailyFoodLogRepository
{
    public ReadDailyFoodLogRepository(DataContext context) : base(context)
    {
    }
}