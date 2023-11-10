using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Entities;
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

	public async Task<IResultWithDataDto<BranchDto>> AddAsync(BranchDto writeBranchDto)
	{
		IResultWithDataDto<BranchDto> res = new ResultWithDataDto<BranchDto>();
		try
		{
			var mapSet = _mapper.Map<Branch>(writeBranchDto);
			var resultData = await _unitOfWork.WriteBranchRepository.AddAsync(mapSet);
			
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<BranchDto>(resultData);
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
		var findData = await _unitOfWork.ReadBranchRepository.GetByIdAsync(id);
		var data = await findData.FirstOrDefaultAsync();
		if (data is null) return false;
		await _unitOfWork.WriteBranchRepository.DeleteAsync(data);
		var resultCommit = _unitOfWork.Commit();
		if (!resultCommit)
			return false;
		return true;
	}

	public async Task<bool> RemoveAsync(int id)
	{
		var findData = await _unitOfWork.ReadBranchRepository.GetByIdAsync(id);
		var data = await findData.FirstOrDefaultAsync();
		if (data is null) return false;
		await _unitOfWork.WriteBranchRepository.RemoveAsync(data);
		var resultCommit = _unitOfWork.Commit();
		if (!resultCommit)
			return false;
		return true;
	}

	public async Task<IResultWithDataDto<BranchDto>> UpdateAsync(BranchDto writeBranchDto)
	{
		IResultWithDataDto<BranchDto> res = new ResultWithDataDto<BranchDto>();
		try
		{
			var findData = await _unitOfWork.ReadBranchRepository.GetByIdAsync(writeBranchDto.ID);
			var getData = await findData.FirstOrDefaultAsync();
			if (getData is null)
				return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			var mapset = _mapper.Map<Branch>(writeBranchDto);
			mapset.ID = getData.ID;
			mapset.CreatedAt = getData.CreatedAt;
			
			var resultData = await _unitOfWork.WriteBranchRepository.Update(mapset);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
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