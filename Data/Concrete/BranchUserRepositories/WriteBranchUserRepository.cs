using Core.Entities;
using Data.Abstract.BranchUserRepositories;
using Data.Context;

namespace Data.Concrete.BranchUserRepositories;

public class WriteBranchUserRepository : WriteRepository<BranchUser> , IWriteBranchUserRepository
{
    public WriteBranchUserRepository(DataContext context) : base(context)
    {
    }
}