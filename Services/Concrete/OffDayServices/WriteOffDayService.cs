using AutoMapper;
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

	public async Task<bool> DeleteAsync(int id)
	{
		var findData = await _unitOfWork.ReadOffDayRepository.GetByIdAsync(id);
		if (findData.FirstOrDefault() is null) return false;
		await _unitOfWork.WriteOffDayRepository.DeleteAsync(findData.First());
		var resultCommit = _unitOfWork.Commit();
		if (!resultCommit)
			return false;
		return true;
	}

	public async Task<bool> RemoveAsync(int id)
	{
		var findData = await _unitOfWork.ReadOffDayRepository.GetByIdAsync(id);
		if (findData.FirstOrDefault() is null) return false;
		await _unitOfWork.WriteOffDayRepository.RemoveAsync(findData.First());
		var resultCommit = _unitOfWork.Commit();
		if (!resultCommit)
			return false;
		return true;
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
}