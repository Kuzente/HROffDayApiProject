using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.PersonalServices;

namespace Services.Concrete.PersonalServices;

public class WritePersonalService : IWritePersonalService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public WritePersonalService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<IResultDto> AddAsync(AddPersonalDto writePersonalDto)
	{
		IResultDto res = new ResultDto();
		try
		{
			var mapSet = _mapper.Map<Personal>(writePersonalDto);
			var resultData = await _unitOfWork.WritePersonalRepository.AddAsync(mapSet);
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

	public async Task<IResultDto> AddRangeAsync(List<AddRangePersonalDto> writeDto)
	{
		IResultDto res = new ResultDto();
		try
		{
			var mapSet = _mapper.Map<List<Personal>>(writeDto);
			var resultData = await _unitOfWork.WritePersonalRepository.AddRangeAsync(mapSet);
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

	public async Task<IResultDto> UpdateAsync(WriteUpdatePersonalDto writeDto)
	{
		IResultDto result = new ResultDto();
		try
		{
			var getPersonal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate:p=> p.ID == writeDto.ID);
			if (getPersonal is null)
				return result.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			var mapSet = _mapper.Map<Personal>(writeDto);
			mapSet.ID = getPersonal.ID;
			mapSet.CreatedAt = getPersonal.CreatedAt;
			var resultData = await _unitOfWork.WritePersonalRepository.Update(mapSet);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<WritePersonalDto>(resultData);
		}
		catch (Exception ex)
		{
			result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return result;
	}
	

	public async Task<IResultDto> DeleteAsync(Guid id)
	{
		IResultDto res = new ResultDto();
		try
		{
			var findData = await _unitOfWork.ReadPersonalRepository.GetByIdAsync(id);
			var data = await findData.FirstOrDefaultAsync();
			if (data is null)  return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");;
			await _unitOfWork.WritePersonalRepository.DeleteAsync(data);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");;
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}
	
	public async Task<IResultDto> ChangeStatus(Guid id)
	{
		IResultDto res = new ResultDto();
		try
		{
			var findData = await _unitOfWork.ReadPersonalRepository.GetByIdAsync(id);
			var data = await findData.FirstOrDefaultAsync();
			if (data is null) return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			if (data.Status == EntityStatusEnum.Online)
			{
				// Eğer "online" ise "offline" yapın
				data.Status = EntityStatusEnum.Offline;
				data.EndJobDate = DateTime.Now;
			}
			else if (data.Status == EntityStatusEnum.Offline)
			{
				// Eğer "offline" ise "online" yapın
				data.Status = EntityStatusEnum.Online;
				data.StartJobDate = DateTime.Now;
				data.EndJobDate = null;
			}

			await _unitOfWork.WritePersonalRepository.Update(data);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");;
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultDto> RecoverAsync(Guid id)
	{
		IResultDto res = new ResultDto();
		try
		{
			var result = await _unitOfWork.WritePersonalRepository.RecoverAsync(id);
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
}