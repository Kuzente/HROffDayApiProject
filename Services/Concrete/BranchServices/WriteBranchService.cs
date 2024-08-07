using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.BranchServices;

namespace Services.Concrete.BranchServices;

public class WriteBranchService : IWriteBranchService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public WriteBranchService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<IResultDto> AddAsync(BranchDto writeBranchDto,Guid userId,string ipAddress)
	{
        IResultDto res = new ResultDto();
		try
		{
			var mapSet = _mapper.Map<Branch>(writeBranchDto);
			await _unitOfWork.WriteBranchRepository.AddAsync(mapSet);
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Branch",
				LogType = LogType.Add,
				Description = $"{mapSet.Name} adlı Şube Eklendi",
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
			var getResult = await _unitOfWork.ReadBranchRepository.GetSingleAsync(predicate: p => p.ID == id , include:p=> p.Include(a=>a.Personals));
			if(getResult is null) return res.SetStatus(false).SetErr("Branch Not Found").SetMessage("Şube Bulunamadı");
			if (getResult.Personals.Any()) return res.SetStatus(false).SetErr("Branch have Personals").SetMessage("Silmek İstediğiniz Şube Altında Personeller Mevcut Lütfen Nakil İşlemlerini Yaptıktan Sonra Tekrar Deneyiniz.");
			
			var result = await _unitOfWork.WriteBranchRepository.DeleteByIdAsync(id);
			if (!result) return res.SetStatus(false).SetErr("Data Layer Error").SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Branch",
				LogType = LogType.Delete,
				Description = $"{getResult.Name} adlı Şube Silindi",
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
			var getResult = await _unitOfWork.ReadBranchRepository.GetSingleAsync(predicate: p => p.ID == id);
			if(getResult is null) return res.SetStatus(false).SetErr("Branch Not Found").SetMessage("Şube Bulunamadı");
			var result = await _unitOfWork.WriteBranchRepository.RecoverAsync(id);
			if (!result) res.SetStatus(false).SetErr("Data Layer Error").SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Branch",
				LogType = LogType.Recover,
				Description = $"{getResult.Name} adlı Şube Geri Döndürüldü.",
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

	public async Task<IResultDto> RemoveAsync(Guid id)
	{
		IResultDto res = new ResultDto();
		
		try
		{
			var result = await _unitOfWork.WriteBranchRepository.RemoveByIdAsync(id);
			if (!result)
				res.SetStatus(false).SetErr("Data Layer Error")
					.SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail")
					.SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<IResultWithDataDto<BranchDto>> UpdateAsync(BranchDto writeBranchDto,Guid userId,string ipAddress)
	{
		IResultWithDataDto<BranchDto> res = new ResultWithDataDto<BranchDto>();
		try
		{
			var findData = await _unitOfWork.ReadBranchRepository.GetByIdAsync(writeBranchDto.ID);
			var getData = await findData.FirstOrDefaultAsync();
			if (getData is null) return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Şube Bulunamadı!!!");
			var mapset = _mapper.Map<Branch>(writeBranchDto);
			mapset.ID = getData.ID;
			mapset.CreatedAt = getData.CreatedAt;
			if (getData != mapset)
			{
				Console.WriteLine("deneme");
			}
			var changes = new List<string>();
			if (getData.Name != mapset.Name)
			{
				changes.Add($"Ad: '{getData.Name}' => '{mapset.Name}'");
			}
			var resultData = await _unitOfWork.WriteBranchRepository.Update(mapset);
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "Branch",
				LogType = LogType.Update,
				Description = $"{resultData.Name} adlı Şube Güncellendi.",
				IpAddress = ipAddress,
				UserID = userId,
			});
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit) return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<BranchDto>(resultData);
			res.SetData(mapResult);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
			
		}
		return res;
	}
}