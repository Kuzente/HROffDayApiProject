using Core.Entities;
using Data.Abstract.UserRepositories;
using Data.Context;

namespace Data.Concrete.UserRepositories;

public class ReadUserRepository: ReadRepository<User>, IReadUserRepository
{
    public ReadUserRepository(DataContext context) : base(context)
    {
    }
}