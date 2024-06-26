﻿using AutoMapper;
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
			var offDay = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(predicate: p => p.ID == dto.ID && p.Personal_Id == dto.Personal_Id && p.Personal.Status == EntityStatusEnum.Online,
				include:p=>p.Include(a=>a.Personal));
			if(offDay.Personal is null)
				return result.SetStatus(false).SetErr("Personal Is Not Found").SetMessage("İlgili Personel Bulunamadı.");
			if((offDay.Personal.TotalYearLeave - offDay.Personal.UsedYearLeave) < dto.LeaveByYear)
				return result.SetStatus(false).SetErr("Personal Total Year Leave Not Enought").SetMessage("Personelin yıllık izini yetersiz.Lütfen daha küçük bir değer giriniz");
			var mappedResult = _mapper.Map<OffDay>(dto);
			mappedResult.PdfRemainTakenLeave = offDay.PdfRemainTakenLeave;
			mappedResult.PdfUsedYearLeave = offDay.PdfUsedYearLeave;
			mappedResult.PdfRemainYearLeave = offDay.PdfRemainYearLeave;
			if (offDay.OffDayStatus == OffDayStatusEnum.Approved) // Eğer İzin Onaylanmış ve Güncelleme Yapılıyor İse
			{
				var personal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p =>
					p.ID == dto.Personal_Id && p.Status == EntityStatusEnum.Online);
				if (personal is null)
					return result.SetStatus(false).SetErr("Personal Not Found").SetMessage("İlgili Personel Bulunamadı.");
				
				mappedResult.OffDayStatus = OffDayStatusEnum.Approved;
				if (dto.LeaveByYear > offDay.LeaveByYear)
				{
					personal.UsedYearLeave += (dto.LeaveByYear - offDay.LeaveByYear);
					mappedResult.PdfUsedYearLeave += (dto.LeaveByYear - offDay.LeaveByYear);
					mappedResult.PdfRemainYearLeave -= (dto.LeaveByYear - offDay.LeaveByYear);
				}
				else if (dto.LeaveByYear < offDay.LeaveByYear)
				{
					personal.UsedYearLeave -= (offDay.LeaveByYear - dto.LeaveByYear);
					mappedResult.PdfUsedYearLeave -= (offDay.LeaveByYear - dto.LeaveByYear);
					mappedResult.PdfRemainYearLeave += (offDay.LeaveByYear - dto.LeaveByYear);
				}
				else if (dto.LeaveByTaken > offDay.LeaveByTaken)
				{
					personal.TotalTakenLeave -= ((dto.LeaveByTaken - offDay.LeaveByTaken) * 8);
					mappedResult.PdfRemainTakenLeave -= ((dto.LeaveByTaken - offDay.LeaveByTaken) * 8);
				}
				else if (dto.LeaveByTaken < offDay.LeaveByTaken)
				{
					personal.TotalTakenLeave += ((offDay.LeaveByTaken - dto.LeaveByTaken) * 8);
					mappedResult.PdfRemainTakenLeave += ((offDay.LeaveByTaken - dto.LeaveByTaken) * 8);
				}

				await _unitOfWork.WritePersonalRepository.Update(personal);
			}
			
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

	public async Task<IResultDto> UpdateFirstWaitingStatusOffDayService(Guid id,bool status,string username)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(predicate: p => p.ID == id);
			if(offday is null)
				return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			offday.OffDayStatus = status ? OffDayStatusEnum.WaitingForSecond : OffDayStatusEnum.Rejected;
			offday.HrName = username;
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

	public async Task<IResultDto> UpdateSecondWaitingStatusOffDayService(Guid id, bool status,string username)
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
					offday.PdfUsedYearLeave = offday.Personal.UsedYearLeave;
					offday.PdfRemainYearLeave = (offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave);
				if (offday.LeaveByTaken > 0)
					offday.Personal.TotalTakenLeave -= (offday.LeaveByTaken * 8);
					offday.PdfRemainTakenLeave = offday.Personal.TotalTakenLeave;
				var getMaxDocNumber = _unitOfWork.ReadOffDayRepository
					.GetAll(orderBy: p => p.OrderByDescending(a => a.DocumentNumber)).FirstOrDefault();
				if(getMaxDocNumber is null)
					return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("Belge Sayısı İşlenirken bir hata meydana geldi.");
				offday.DocumentNumber = getMaxDocNumber.DocumentNumber + 1;
				offday.OffDayStatus = OffDayStatusEnum.Approved;
				offday.DirectorName = username;
			}
			else
			{
				offday.DirectorName = username;
				offday.OffDayStatus = OffDayStatusEnum.Rejected;
			}
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
				predicate:p=> p.ID == id && p.Status == EntityStatusEnum.Online && p.Personal.Status == EntityStatusEnum.Online,
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