using Core.Entities;
using Data.Abstract.BranchRepositories;
using Data.Context;

namespace Data.Concrete.BranchRepositories;

public class WriteBranchRepository : WriteRepository<Branch>, IWriteBranchRepository
{
	public WriteBranchRepository(DataContext context) : base(context)
	{
	}
}