using Core.Entities;
using Data.Abstract.UserLogRepositories;
using Data.Context;

namespace Data.Concrete.UserLogRepositories;

public class ReadUserLogRepository: ReadRepository<UserLog>, IReadUserLogRepository
{
    public ReadUserLogRepository(DataContext context) : base(context)
    {
    }
}