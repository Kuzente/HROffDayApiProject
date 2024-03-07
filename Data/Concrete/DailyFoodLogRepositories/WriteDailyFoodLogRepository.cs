using Core.Entities;
using Data.Abstract.DailyFoodLogRepositories;
using Data.Context;

namespace Data.Concrete.DailyFoodLogRepositories;

public class WriteDailyFoodLogRepository : WriteRepository<DailyFoodLog>, IWriteDailyFoodLogRepository
{
    public WriteDailyFoodLogRepository(DataContext context) : base(context)
    {
    }
}