using Core.Entities;
using Data.Abstract.DailyCounterRepositories;
using Data.Context;

namespace Data.Concrete.DailyCounterRepositories;

public class WriteDailyCounterRepository : WriteRepository<DailyCounter>, IWriteDailyCounterRepository
{
    public WriteDailyCounterRepository(DataContext context) : base(context)
    {
    }
}