using Core.Entities;
using Data.Abstract.MissingDayRepositories;
using Data.Context;

namespace Data.Concrete.MissingDayRepositories;

public class WriteMissingDayRepository : WriteRepository<MissingDay>, IWriteMissingDayRepository
{
    public WriteMissingDayRepository(DataContext context) : base(context)
    {
    }
}