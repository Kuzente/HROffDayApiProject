using Core.Entities;
using Data.Abstract.OffDayRepositories;
using Data.Context;

namespace Data.Concrete.OffDayRepositories;

public class WriteOffDayRepository : WriteRepository<OffDay>, IWriteOffDayRepository
{
	public WriteOffDayRepository(DataContext context) : base(context)
	{
	}
}