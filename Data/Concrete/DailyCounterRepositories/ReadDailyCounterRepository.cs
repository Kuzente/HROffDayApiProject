using Core.Entities;
using Data.Abstract.DailyCounterRepositories;
using Data.Context;

namespace Data.Concrete.DailyCounterRepositories;

public class ReadDailyCounterRepository : ReadRepository<DailyCounter>, IReadDailyCounterRepository
{
    public ReadDailyCounterRepository(DataContext context) : base(context)
    {
    }
}