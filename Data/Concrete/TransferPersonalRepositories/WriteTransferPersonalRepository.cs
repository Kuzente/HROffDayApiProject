using Core.Entities;
using Data.Abstract.TransferPersonalRepositories;
using Data.Context;

namespace Data.Concrete.TransferPersonalRepositories;

public class WriteTransferPersonalRepository : WriteRepository<TransferPersonal>, IWriteTransferPersonalRepository
{
    public WriteTransferPersonalRepository(DataContext context) : base(context)
    {
    }
}