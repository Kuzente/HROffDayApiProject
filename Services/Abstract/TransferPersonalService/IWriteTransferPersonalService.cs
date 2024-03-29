using Core.Interfaces;

namespace Services.Abstract.TransferPersonalService;

public interface IWriteTransferPersonalService
{
    Task<IResultDto> DeleteTransferPersonalService(Guid id);
}