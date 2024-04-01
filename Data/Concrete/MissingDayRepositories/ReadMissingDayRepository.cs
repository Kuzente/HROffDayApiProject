using Core.Entities;
using Data.Abstract.MissingDayRepositories;
using Data.Context;

namespace Data.Concrete.MissingDayRepositories;

public class ReadMissingDayRepository : ReadRepository<MissingDay>, IReadMissingDayRepository
{
    public ReadMissingDayRepository(DataContext context) : base(context)
    {
    }
}