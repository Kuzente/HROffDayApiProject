using Core;
using Core.DTOs.MissingDayDtos.WriteDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.MissingDayServices;

namespace Services.Concrete.MissingDayServices;

public class WriteMissingDayService : IWriteMissingDayService
{
    private readonly IUnitOfWork _unitOfWork;

    public WriteMissingDayService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResultDto> AddMissingDayService(WriteAddMissingDayDto dto,Guid userId,string ipAddress)
    {
        IResultDto result = new ResultDto();
        try
        {
            if(dto.StartOffdayDate > dto.EndOffDayDate || (dto.StartJobDate.HasValue && dto.EndOffDayDate >= dto.StartJobDate.Value))
                return result.SetStatus(false).SetErr("Datetime Error").SetMessage("Lütfen Girdiğiniz Tarihleri Kontrol ediniz.");
            var queryPersonal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p =>
                p.Status == EntityStatusEnum.Online &&
                p.ID == dto.PersonalId);
            if(queryPersonal is null) return result.SetStatus(false).SetErr("Personal is not found").SetMessage("İlgili Personel Bulunamadı.");
            var addingMissDay = new MissingDay
            {
                Personal_Id = dto.PersonalId,
                Reason = dto.Reason,
                Branch_Id = queryPersonal!.Branch_Id,
                StartOffdayDate = dto.StartOffdayDate,
                EndOffDayDate = dto.EndOffDayDate,
                StartJobDate = dto.StartJobDate,
            };
            await _unitOfWork.WriteMissingDayRepository.AddAsync(addingMissDay);
           
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "MissingDay",
                LogType = LogType.Add,
                Description = $"{queryPersonal.NameSurname} adlı personele Eksik gün kaydı eklendi.",
                IpAddress = ipAddress,
                UserID = userId,
            });
            var resultCommit = _unitOfWork.Commit();
            if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");

        }
        catch (Exception e)
        {
            result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }

    public async Task<IResultDto> DeleteMissingDayService(Guid id,Guid userId,string ipAddress)
    {
        IResultDto result = new ResultDto();
        try
        {
            var data = await _unitOfWork.ReadMissingDayRepository.GetSingleAsync(
                predicate:p=> 
                    p.ID == id && 
                    p.Status == EntityStatusEnum.Online&&
                    p.Personal.Status == EntityStatusEnum.Online,
                include:a=>a.Include(p=>p.Personal)
            );
            if(data is null) return result.SetStatus(false).SetErr("MissingDay Data Is Not Found").SetMessage("İlgili Kayıt Bulunamadı.");
            var resultAction = await _unitOfWork.WriteMissingDayRepository.RemoveByIdAsync(data.ID);
            if(!resultAction) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
        
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "MissingDay",
                LogType = LogType.Delete,
                Description = $"{data.Personal.NameSurname} adlı personele ait eksik gün kaydı silindi.",
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