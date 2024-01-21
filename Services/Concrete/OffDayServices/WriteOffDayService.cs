using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
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

	
	

	public async Task<bool> ChangeOffDayStatus(Guid id,bool isApproved)
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

	public async Task<IResultDto> AddOffDayService(WriteAddOffDayDto dto)
	{
		IResultDto result = new ResultDto();
		try
		{
			if(dto.StartDate > dto.EndDate)
				return result.SetStatus(false).SetErr("StartDate is bigger than EndDate").SetMessage("İzin başlangıç tarihi bitişten büyük olamaz!"); 
			if(dto.CountLeave != ((dto.EndDate - dto.StartDate).Days + 1))
				return result.SetStatus(false).SetErr("Date and Count not equal").SetMessage("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor."); 
			var personal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p => p.ID == dto.Personal_Id && p.Status == EntityStatusEnum.Online);
			if(personal is null)
				return result.SetStatus(false).SetErr("Personal Is Not Found").SetMessage("İlgili Personel Bulunamadı.");
			if((personal.TotalYearLeave - personal.UsedYearLeave) < dto.LeaveByYear)
				return result.SetStatus(false).SetErr("Personal Total Year Leave Not Enought").SetMessage("Personelin yıllık izini yetersiz.Lütfen daha küçük bir değer giriniz");
			var mappedResult = _mapper.Map<OffDay>(dto);
			if (dto.LeaveByMarriedFatherDead is not null)
			{
				dto.LeaveByMarriedFatherDead.ForEach(a =>
				{
					if (a.Contains("LeaveByFather"))
						mappedResult.LeaveByFather = 5;
					else if (a.Contains("LeaveByDead"))
						mappedResult.LeaveByDead = 3;
					else if (a.Contains("LeaveByMarried"))
						mappedResult.LeaveByMarried = 3;
				});
			}

			await _unitOfWork.WriteOffDayRepository.AddAsync(mappedResult);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");

		}
		catch (Exception ex)
		{
			result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}

	public async Task<IResultDto> UpdateWaitingOffDayService(WriteUpdateWatingOffDayDto dto)
	{
		IResultDto result = new ResultDto();
		try
		{
			if(dto.StartDate > dto.EndDate)
				return result.SetStatus(false).SetErr("StartDate is bigger than EndDate").SetMessage("İzin başlangıç tarihi bitişten büyük olamaz!"); 
			if(dto.CountLeave != ((dto.EndDate - dto.StartDate).Days + 1))
				return result.SetStatus(false).SetErr("Date and Count not equal").SetMessage("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor."); 
			var personal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p => p.ID == dto.Personal_Id && p.Status == EntityStatusEnum.Online);
			if(personal is null)
				return result.SetStatus(false).SetErr("Personal Is Not Found").SetMessage("İlgili Personel Bulunamadı.");
			if((personal.TotalYearLeave - personal.UsedYearLeave) < dto.LeaveByYear)
				return result.SetStatus(false).SetErr("Personal Total Year Leave Not Enought").SetMessage("Personelin yıllık izini yetersiz.Lütfen daha küçük bir değer giriniz");
			var mappedResult = _mapper.Map<OffDay>(dto);
			if (dto.LeaveByMarriedFatherDead is not null)
			{
				dto.LeaveByMarriedFatherDead.ForEach(a =>
				{
					if (a.Contains("LeaveByFather"))
						mappedResult.LeaveByFather = 5;
					else if (a.Contains("LeaveByDead"))
						mappedResult.LeaveByDead = 3;
					else if (a.Contains("LeaveByMarried"))
						mappedResult.LeaveByMarried = 3;
				});
			}
			await _unitOfWork.WriteOffDayRepository.Update(mappedResult);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception e)
		{
			result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}

	public async Task<IResultDto> UpdateFirstWaitingStatusOffDayService(Guid id,bool status)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(predicate: p => p.ID == id);
			if(offday is null)
				return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			offday.OffDayStatus = status ? OffDayStatusEnum.WaitingForSecond : OffDayStatusEnum.Rejected;
			await _unitOfWork.WriteOffDayRepository.Update(offday);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception e)
		{
			result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}

	public async Task<IResultDto> UpdateSecondWaitingStatusOffDayService(Guid id, bool status)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => p.ID == id,
				include: p=> p.Include(a=>a.Personal)
				);
			if(offday is null)
				return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (status)
			{
				if (offday.LeaveByYear > 0)
					offday.Personal.UsedYearLeave += offday.LeaveByYear;
				if (offday.LeaveByTaken > 0)
					offday.Personal.TotalTakenLeave -= (offday.LeaveByTaken * 8);
				offday.OffDayStatus = OffDayStatusEnum.Approved;
			}
			else
				offday.OffDayStatus = OffDayStatusEnum.Rejected;
			await _unitOfWork.WriteOffDayRepository.Update(offday);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception e)
		{
			result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}

	public async Task<IResultDto> DeleteOffDayService(Guid id)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offDay = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate:p=> p.ID == id && p.Status == EntityStatusEnum.Online,
				include: p=> p.Include(a=> a.Personal)
			);
			if(offDay is null)
				return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (offDay.LeaveByYear > 0) // Alınan yıllık izini geri ata
				offDay.Personal.UsedYearLeave -= offDay.LeaveByYear;
			if (offDay.LeaveByTaken > 0) // Alınan alacak iznini geri ata
				offDay.Personal.TotalTakenLeave += (offDay.LeaveByTaken * 8);
			offDay.Status = EntityStatusEnum.Deleted;
			offDay.DeletedAt = DateTime.Now;
			await _unitOfWork.WriteOffDayRepository.Update(offDay);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception e)
		{
			result.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}
}