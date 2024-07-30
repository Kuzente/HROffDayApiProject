using Core.Entities;
using Data.Abstract.UserLogRepositories;
using Data.Context;

namespace Data.Concrete.UserLogRepositories;

public class WriteUserLogRepository: WriteRepository<UserLog>, IWriteUserLogRepository
{
    public WriteUserLogRepository(DataContext context) : base(context)
    {
    }
}