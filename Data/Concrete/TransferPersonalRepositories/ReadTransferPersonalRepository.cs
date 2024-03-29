using Core.Entities;
using Data.Abstract;
using Data.Abstract.TransferPersonalRepositories;
using Data.Context;

namespace Data.Concrete.TransferPersonalRepositories;

public class ReadTransferPersonalRepository : ReadRepository<TransferPersonal>, IReadTransferPersonalRepository
{
    public ReadTransferPersonalRepository(DataContext context) : base(context)
    {
    }
}