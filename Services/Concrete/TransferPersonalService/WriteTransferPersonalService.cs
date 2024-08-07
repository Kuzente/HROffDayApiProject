using Core;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.TransferPersonalService;

namespace Services.Concrete.TransferPersonalService;

public class WriteTransferPersonalService : IWriteTransferPersonalService
{
    private readonly IUnitOfWork _unitOfWork;

    public WriteTransferPersonalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResultDto> DeleteTransferPersonalService(Guid id,Guid userId,string ipAddress)
    {
        IResultDto result = new ResultDto();
        try
        {
            var data = await _unitOfWork.ReadTransferPersonalRepository.GetSingleAsync(
                predicate:p=> p.ID == id && p.Status == EntityStatusEnum.Online,
                include: p=> p.Include(a=>a.Personal)
            );
            if(data is null) return result.SetStatus(false).SetErr("TransferPersonal Data Is Not Found").SetMessage("İlgili Kayıt Bulunamadı.");
            var resultAction = await _unitOfWork.WriteTransferPersonalRepository.RemoveByIdAsync(data.ID);
            if(!resultAction) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "TransferPersonals",
                LogType = LogType.Delete,
                Description = $"{data.Personal.NameSurname} adlı personele ait görevlendirme kaydı silindi.",
                IpAddress = ipAddress,
                UserID = userId,
            });
            var resultCommit = _unitOfWork.Commit();
            if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
        }
        catch (Exception ex)
        {
            result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }
}