using Core.Entities;
using Data.Abstract.BranchUserRepositories;
using Data.Context;

namespace Data.Concrete.BranchUserRepositories;

public class ReadBranchUserRepository : ReadRepository<BranchUser>,IReadBranchUserRepository
{
    public ReadBranchUserRepository(DataContext context) : base(context)
    {
    }
}