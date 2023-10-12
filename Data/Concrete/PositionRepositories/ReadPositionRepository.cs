using Core.Entities;
using Data.Abstract.PositionRepositories;
using Data.Context;

namespace Data.Concrete.PositionRepositories
{
	public class ReadPositionRepository : ReadRepository<Position>, IReadPositionRepository
	{
		public ReadPositionRepository(DataContext context) : base(context)
		{
		}
	}
}
