using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.PositionServices;

namespace Services.Concrete.PositionServices;

public class WritePositionService : IWritePositionService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public WritePositionService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}
	public async Task<IResultDto> AddAsync(PositionDto writePositionDto,Guid userId,string ipAddress)
	{
        IResultDto res = new ResultDto();
		try
		{
			var mapSet = _mapper.Map<Position>(writePositionDto);
			var resultData = await _unitOfWork.WritePositionRepository.AddAsync(mapSet);
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Position",
				LogType = LogType.Add,
				Description = $"{mapSet.Name} adlı Ünvan Eklendi",
				IpAddress = ipAddress,
				UserID = userId,
			});
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<IResultDto> DeleteAsync(Guid id,Guid userId,string ipAddress)
	{
		IResultDto res = new ResultDto();
		try
		{
			var getResult = await _unitOfWork.ReadPositionRepository.GetSingleAsync(predicate: p => p.ID == id , include:p=> p.Include(a=>a.Personals));
			if(getResult is null) return res.SetStatus(false).SetErr("Position Not Found").SetMessage("Ünvan Bulunamadı");
			if (getResult.Personals.Any()) return res.SetStatus(false).SetErr("Position have Personals").SetMessage("Silmek İstediğiniz Ünvan Altında Personeller Mevcut Lütfen Nakil İşlemlerini Yaptıktan Sonra Tekrar Deneyiniz.");
			var result = await _unitOfWork.WritePositionRepository.DeleteByIdAsync(id);
			if (!result) res.SetStatus(false).SetErr("Data Layer Error").SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Position",
				LogType = LogType.Delete,
				Description = $"{getResult.Name} adlı Ünvan Silindi",
				IpAddress = ipAddress,
				UserID = userId,
			});
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit) return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<IResultDto> RecoverAsync(Guid id,Guid userId,string ipAddress)
	{
		IResultDto res = new ResultDto();
		try
		{
			var getResult = await _unitOfWork.ReadPositionRepository.GetSingleAsync(predicate: p => p.ID == id);
			if(getResult is null) return res.SetStatus(false).SetErr("Position Not Found").SetMessage("Ünvan Bulunamadı");
			var result = await _unitOfWork.WritePositionRepository.RecoverAsync(id);
			if (!result) res.SetStatus(false).SetErr("Data Layer Error").SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Position",
				LogType = LogType.Recover,
				Description = $"{getResult.Name} adlı Ünvan Geri Döndürüldü",
				IpAddress = ipAddress,
				UserID = userId,
			});
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit) return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}
	

	public async Task<IResultWithDataDto<PositionDto>> UpdateAsync(PositionDto writeBranchDto,Guid userId,string ipAddress)
	{
        IResultWithDataDto<PositionDto> res = new ResultWithDataDto<PositionDto>();
        try
        {
            var getdataQuary = await _unitOfWork.ReadPositionRepository.GetByIdAsync(writeBranchDto.ID);
            var getData = await getdataQuary.FirstOrDefaultAsync();
            if (getData is null) return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
            var mapset = _mapper.Map<Position>(writeBranchDto);
            mapset.ID = getData.ID;
            mapset.CreatedAt = getData.CreatedAt;
            var resultData = await _unitOfWork.WritePositionRepository.Update(mapset);
         
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
	            EntityName = "Position",
	            LogType = LogType.Update,
	            Description = $"{resultData.Name} adlı Ünvan Güncellendi",
	            IpAddress = ipAddress,
	            UserID = userId,
            });
            var resultCommit = _unitOfWork.Commit();
            if (!resultCommit) return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
            var mapResult = _mapper.Map<PositionDto>(resultData);
            res.SetData(mapResult);
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");

        }
        return res;
    }
}