using Core.Entities;
using Data.Abstract.UserRepositories;
using Data.Context;

namespace Data.Concrete.UserRepositories;

public class WriteUserRepository: WriteRepository<User>, IWriteUserRepository
{
    public WriteUserRepository(DataContext context) : base(context)
    {
    }
}