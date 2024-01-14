using AutoMapper;
using Core;
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

	public async Task<IResultDto> AddAsync(BranchDto writeBranchDto)
	{
        IResultDto res = new ResultDto();
		try
		{
			var mapSet = _mapper.Map<Branch>(writeBranchDto);
			var resultData = await _unitOfWork.WriteBranchRepository.AddAsync(mapSet);
			
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

	public async Task<IResultDto> DeleteAsync(Guid id)
	{
		IResultDto res = new ResultDto();
		
		try
		{
			var result = await _unitOfWork.WriteBranchRepository.DeleteByIdAsync(id);
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

	public async Task<IResultDto> RecoverAsync(Guid id)
	{
		IResultDto res = new ResultDto();
		try
		{
			var result = await _unitOfWork.WriteBranchRepository.RecoverAsync(id);
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