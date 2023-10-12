using Core.Entities;
using Data.Abstract.BranchRepositories;
using Data.Context;

namespace Data.Concrete.BranchRepositories
{
	public class ReadBranchRepository : ReadRepository<Branch>, IReadBranchRepository
	{
		public ReadBranchRepository(DataContext context) : base(context)
		{
		}
	}
}
