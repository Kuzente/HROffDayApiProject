using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.PersonalDTOs;
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

	public async Task<IResultWithDataDto<WritePersonalDto>> UpdateAsync(WritePersonalDto writePersonalDto)
	{
		IResultWithDataDto<WritePersonalDto> res = new ResultWithDataDto<WritePersonalDto>();
		try
		{
			var getDataQuery = await _unitOfWork.ReadPersonalRepository.GetByIdAsync(writePersonalDto.ID);
			var getData = await getDataQuery.FirstOrDefaultAsync();
			if (getData is null)
				return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			var mapSet = _mapper.Map<Personal>(writePersonalDto);
			mapSet.ID = getData.ID;
			mapSet.CreatedAt = getData.CreatedAt;
			var resultData = await _unitOfWork.WritePersonalRepository.Update(mapSet);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<WritePersonalDto>(resultData);
			res.SetData(mapResult);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<IResultDto> DeleteAsync(int id)
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

	public Task<bool> RemoveAsync(int id)
	{
		throw new NotImplementedException(); //TODO
	}

	public async Task<IResultDto> ChangeStatus(int id)
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
			}
			else if (data.Status == EntityStatusEnum.Offline)
			{
				// Eğer "offline" ise "online" yapın
				data.Status = EntityStatusEnum.Online;
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

	public async Task<IResultDto> RecoverAsync(int id)
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