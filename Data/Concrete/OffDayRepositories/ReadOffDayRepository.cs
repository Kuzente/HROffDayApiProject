using Core.Entities;
using Data.Abstract.OffDayRepositories;
using Data.Context;

namespace Data.Concrete.OffDayRepositories;

public class ReadOffDayRepository : ReadRepository<OffDay>, IReadOffDayRepository
{
	public ReadOffDayRepository(DataContext context) : base(context)
	{
	}
}