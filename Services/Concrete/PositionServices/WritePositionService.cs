﻿using AutoMapper;
using Core.DTOs.BranchDTOs;
using Core.DTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.PositionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete.PositionServices
{
	public class WritePositionService : IWritePositionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public WritePositionService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<IResultWithDataDto<ReadPositionDto>> AddAsync(WritePositionDto writePositionDto)
		{
			IResultWithDataDto<ReadPositionDto> res = new ResultWithDataDto<ReadPositionDto>();
			try
			{
				var mapSet = _mapper.Map<Position>(writePositionDto);
				var resultData = await _unitOfWork.WritePositionRepository.AddAsync(mapSet);
				var resultCommit = _unitOfWork.Commit();
				if (!resultCommit)
					return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
				var mapResult = _mapper.Map<ReadPositionDto>(resultData);
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
			var findData = await _unitOfWork.ReadPositionRepository.GetByIdAsync(Id);
			await _unitOfWork.WritePositionRepository.DeleteAsync(findData);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return false;
			return true;
		}

		public async Task<bool> RemoveAsync(int Id)
		{
			var findData = await _unitOfWork.ReadPositionRepository.GetByIdAsync(Id);
			await _unitOfWork.WritePositionRepository.RemoveAsync(findData);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return false;
			return true;
		}

		public async Task<IResultWithDataDto<ReadPositionDto>> UpdateAsync(WritePositionDto writeBranchDto)
		{
			IResultWithDataDto<ReadPositionDto> res = new ResultWithDataDto<ReadPositionDto>();
			try
			{
				var mapset = _mapper.Map<Position>(writeBranchDto);
				var resultData = await _unitOfWork.WritePositionRepository.Update(mapset);
				var resultCommit = _unitOfWork.Commit();
				if (!resultCommit)
					return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
				var mapResult = _mapper.Map<ReadPositionDto>(resultData);
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
