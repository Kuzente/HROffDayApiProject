using Core.Entities;
using Data.Abstract.PositionRepositories;
using Data.Context;

namespace Data.Concrete.PositionRepositories
{
	public class WritePositionRepository : WriteRepository<Position>, IWritePositionRepository
	{
		public WritePositionRepository(DataContext context) : base(context)
		{
		}
	}
}
