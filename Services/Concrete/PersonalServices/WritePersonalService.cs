using AutoMapper;
using Core.DTOs;
using Core.DTOs.PersonalDTOs;
using Core.Entities;
using Core.Interfaces;
using Data.Abstract;
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

	public async Task<IResultWithDataDto<ReadPersonalDto>> AddAsync(WritePersonalDto writePersonalDto)
	{
		IResultWithDataDto<ReadPersonalDto> res = new ResultWithDataDto<ReadPersonalDto>();
		try
		{
			var mapSet = _mapper.Map<Personal>(writePersonalDto);
			var resultData = await _unitOfWork.WritePersonalRepository.AddAsync(mapSet);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<ReadPersonalDto>(resultData);
			res.SetData(mapResult);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}
	public async Task<IResultWithDataDto<ReadPersonalDto>> UpdateAsync(WritePersonalDto writePersonalDto)
	{
		IResultWithDataDto<ReadPersonalDto> res = new ResultWithDataDto<ReadPersonalDto>();
		try
		{
			var mapset = _mapper.Map<Personal>(writePersonalDto);
			var resultData = await _unitOfWork.WritePersonalRepository.Update(mapset);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			var mapResult = _mapper.Map<ReadPersonalDto>(resultData);
			res.SetData(mapResult);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public Task<bool> DeleteAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<bool> RemoveAsync(int id)
	{
		throw new NotImplementedException();
	}

		
}