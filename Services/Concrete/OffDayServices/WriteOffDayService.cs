using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.OffDayServices;

namespace Services.Concrete.OffDayServices;

public class WriteOffDayService : IWriteOffDayService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public WriteOffDayService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<IResultWithDataDto<ReadOffDayDto>> AddAsync(WriteOffDayDto writeDto)
	{
		IResultWithDataDto<ReadOffDayDto> res = new ResultWithDataDto<ReadOffDayDto>();
		try
		{
			var mapSet = _mapper.Map<OffDay>(writeDto);
			mapSet.OffDayStatus = OffDayStatusEnum.WaitingForFirst; // make form request waiting first
			var resultData = await _unitOfWork.WriteOffDayRepository.AddAsync(mapSet);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<ReadOffDayDto>(resultData);
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
			var result = await _unitOfWork.WriteOffDayRepository.DeleteByIdAsync(id);
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

	public Task<IResultDto> RecoverAsync(int id)
	{
		throw new NotImplementedException();
	}

	public async Task<IResultDto> RemoveAsync(int id)
	{
		IResultDto res = new ResultDto();
		
		try
		{
			var result = await _unitOfWork.WriteOffDayRepository.RemoveByIdAsync(id);
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

	public async Task<bool> ChangeOffDayStatus(int id,bool isApproved)
	{
		var findData = await _unitOfWork.ReadOffDayRepository.GetByIdAsync(id);
		if (findData.FirstOrDefault() is null) return false;
		var data = findData.First();
		if (isApproved)
		{
			data.OffDayStatus = OffDayStatusEnum.Approved;
			await _unitOfWork.WriteOffDayRepository.Update(data);
			var resultCommit = _unitOfWork.Commit();
			return resultCommit;
		}
		else
		{
			data.OffDayStatus = OffDayStatusEnum.Rejected;
			await _unitOfWork.WriteOffDayRepository.Update(data);
			var resultCommit = _unitOfWork.Commit();
			return resultCommit;
		}
	}

	public async Task<IResultWithDataDto<ReadOffDayDto>> UpdateAsync(WriteOffDayDto writeDto)
	{
		IResultWithDataDto<ReadOffDayDto> res = new ResultWithDataDto<ReadOffDayDto>();
		try
		{
			var getdata = await _unitOfWork.ReadOffDayRepository.GetByIdAsync(writeDto.ID);
			if (getdata.FirstOrDefault() is null)
				return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			var mapset = _mapper.Map<OffDay>(writeDto);
			var resultData = await _unitOfWork.WriteOffDayRepository.Update(mapset);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<ReadOffDayDto>(resultData);
			res.SetData(mapResult);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

    public Task<IResultWithDataDto<ReadOffDayDto>> AddAsync(ReadOffDayDto writeDto)
    {
        throw new NotImplementedException();
    }

    public Task<IResultWithDataDto<ReadOffDayDto>> UpdateAsync(ReadOffDayDto writeDto)
    {
        throw new NotImplementedException();
    }
}