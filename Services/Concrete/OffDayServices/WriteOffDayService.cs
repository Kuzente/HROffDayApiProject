using AutoMapper;
using Core.DTOs.BranchDTOs;
using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.Entities;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.OffDayServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete.OffDayServices
{
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

		public async Task<bool> DeleteAsync(int Id)
		{
			var findData = await _unitOfWork.ReadOffDayRepository.GetByIdAsync(Id);
			await _unitOfWork.WriteOffDayRepository.DeleteAsync(findData);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return false;
			return true;
		}

		public async Task<bool> RemoveAsync(int Id)
		{
			var findData = await _unitOfWork.ReadOffDayRepository.GetByIdAsync(Id);
			await _unitOfWork.WriteOffDayRepository.RemoveAsync(findData);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return false;
			return true;
		}

		public async Task<IResultWithDataDto<ReadOffDayDto>> UpdateAsync(WriteOffDayDto writeDto)
		{
			IResultWithDataDto<ReadOffDayDto> res = new ResultWithDataDto<ReadOffDayDto>();
			try
			{
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
}
