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
using Services.HelperServices;
using Services.TestMailServices;
using System.Web;

namespace Services.Concrete.OffDayServices;

public class WriteOffDayService : IWriteOffDayService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IEmailService _emailService;

	public WriteOffDayService(IUnitOfWork unitOfWork, IMapper mapper,IEmailService emailService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_emailService = emailService;
	}
	public async Task<IResultDto> AddOffDayService(WriteAddOffDayDto dto,Guid userId,string ipAddress)
	{
		IResultDto result = new ResultDto();
		try
		{
			if(dto.StartDate > dto.EndDate)
				return result.SetStatus(false).SetErr("StartDate is bigger than EndDate").SetMessage("İzin başlangıç tarihi bitişten büyük olamaz!"); 
			if(dto.CountLeave != ((dto.EndDate - dto.StartDate).Days + 1))
				return result.SetStatus(false).SetErr("Date and Count not equal").SetMessage("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor."); 
			var personal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p => p.ID == dto.Personal_Id && p.Status == EntityStatusEnum.Online);
			if(personal is null) return result.SetStatus(false).SetErr("Personal Is Not Found").SetMessage("İlgili Personel Bulunamadı.");
			if((personal.TotalYearLeave - personal.UsedYearLeave) < dto.LeaveByYear) return result.SetStatus(false).SetErr("Personal Total Year Leave Not Enought").SetMessage("Personelin yıllık izini yetersiz.Lütfen daha küçük bir değer giriniz.");
			if (dto.LeaveByTaken > 0 && personal.TotalTakenLeave < dto.LeaveByTaken * 8)
				return result.SetStatus(false).SetErr("Personal Total Taken Leave is not enought").SetMessage("Personelin alacak izin sayısı yetersiz.Lütfen daha küçük bir değer giriniz.");
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
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "OffDay",
				LogType = LogType.OffDayCreate,
				Description = $"{personal.NameSurname} adlı Personele izin talebi oluşturuldu.",
				IpAddress = ipAddress,
				UserID = userId,
			});
			var getBranchManagerMail = await _unitOfWork.ReadBranchUserRepository.GetSingleAsync(
				predicate: p => p.BranchID == mappedResult.BranchId && p.User.Role == UserRoleEnum.BranchManager,
				include: p => p.Include(a => a.User));
			if (getBranchManagerMail is null || getBranchManagerMail.User is null)
				return result.SetStatus(false).SetErr(" BranchManager not found").SetMessage("Personele ait şube sorumlusu aranırken bir hata oluştu!!");
			var hrEmpoyeeList = _unitOfWork.ReadUserRepository.GetAll(predicate: p => p.Role == UserRoleEnum.HumanResources && p.Status == EntityStatusEnum.Online).ToList();
			if(!hrEmpoyeeList.Any())
				return result.SetStatus(false).SetErr("Mail Send Fail").SetMessage("Sistem üzerinde insan kaynakları bulunamadı");
			foreach (var hr in hrEmpoyeeList)
            {
				var mailHtml = await _emailService.GetMailemplateHtml(
				  "waitingoffday",
				  hr.Username,
				  personal.NameSurname,
				  getBranchManagerMail.User.Username,
				  "şube sorumlusu",
				  DateTime.Now);
				var isMailSendSuccess = await _emailService.SendEmailAsync(hr.Email, "İyaş Personel Takip Bekleyen İzin Onayı", mailHtml);
				if (isMailSendSuccess is false || string.IsNullOrEmpty(mailHtml))
					return result.SetStatus(false).SetErr("Mail Send Fail").SetMessage("Eposta Gönderilirken Bir Hata oluştu! Lütfen daha sonra tekrar deneyiniz...");
			}
           
			
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

	public async Task<IResultDto> UpdateWaitingOffDayService(WriteUpdateWatingOffDayDto dto,Guid userId,string ipAddress)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offDay = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => 
			p.ID == dto.ID && 
			p.Personal_Id == dto.Personal_Id && 
			p.Personal.ID == dto.Personal_Id &&
			p.OffDayStatus != OffDayStatusEnum.Approved &&
			p.Personal.Status == EntityStatusEnum.Online,
				include:p=>
				p.Include(a=>a.Personal));
			if(offDay is null)
				return result.SetStatus(false).SetErr("Personal Is Not Found").SetMessage("İlgili Personel Bulunamadı.");
			if((offDay.Personal.TotalYearLeave - offDay.Personal.UsedYearLeave) < dto.LeaveByYear)
				return result.SetStatus(false).SetErr("Personal Year Leave Insufficient").SetMessage("Personelin yıllık izini yetersiz.Lütfen daha küçük bir değer giriniz");
			var mappedResult = _mapper.Map<OffDay>(dto);
			
			if (dto.LeaveByMarriedFatherDead is not null)
			{
				dto.LeaveByMarriedFatherDead.ForEach(a =>
				{
					if (a.Contains("LeaveByFather"))
						mappedResult.LeaveByFather = 5;
					else
						mappedResult.LeaveByFather = 0;

					if (a.Contains("LeaveByDead"))
						mappedResult.LeaveByDead = 3;
					else
						mappedResult.LeaveByDead = 0;
					if (a.Contains("LeaveByMarried"))
						mappedResult.LeaveByMarried = 3;
					else
						mappedResult.LeaveByMarried = 0;
				});
			}
			else
			{
				mappedResult.LeaveByFather = 0;
				mappedResult.LeaveByDead = 0;
				mappedResult.LeaveByMarried = 0;
			}
			await _unitOfWork.WriteOffDayRepository.Update(mappedResult);
		
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "OffDay",
				LogType = LogType.Update,
				Description = $"{offDay.Personal.NameSurname} adlı Personele ait bekleyen izin güncellendi.",
				IpAddress = ipAddress,
				UserID = userId,
			});
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

	public async Task<IResultDto> UpdateApprovedOffDayService(WriteUpdateWatingOffDayDto dto,Guid userId,string ipAddress)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offDay = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => 
					p.ID == dto.ID && 
					p.Personal_Id == dto.Personal_Id && 
					p.Personal.ID == dto.Personal_Id &&
					p.OffDayStatus == OffDayStatusEnum.Approved &&
					p.Personal.Status == EntityStatusEnum.Online,
				include:p=>
					p.Include(a=>a.Personal)
						.ThenInclude(pc => pc.PersonalCumulatives));
			if(offDay is null)
				return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (offDay.Personal is null || !offDay.Personal.PersonalCumulatives.Any())
				return result.SetStatus(false).SetErr("Personal Not Found").SetMessage("İlgili Personel Bulunamadı.");
			if((offDay.Personal.TotalYearLeave - offDay.Personal.UsedYearLeave) + offDay.LeaveByYear < dto.LeaveByYear)
				return result.SetStatus(false).SetErr("Personal Year Leave Insufficient").SetMessage("Personelin yıllık izini yetersiz.Lütfen daha küçük bir değer giriniz");
			
			//Yıllık izin değeri artmış ise
			if (dto.LeaveByYear > offDay.LeaveByYear)
			{
				var addedLeaveYear = dto.LeaveByYear - offDay.LeaveByYear;
				offDay.Personal.UsedYearLeave += addedLeaveYear;
				foreach (var personalCumulative in offDay.Personal.PersonalCumulatives.OrderBy(pc => pc.Year))
				{
					if (personalCumulative.RemainYearLeave > 0)
					{
						if (addedLeaveYear < personalCumulative.RemainYearLeave)
						{
							personalCumulative.RemainYearLeave -= addedLeaveYear;
							break;
						}
						if(addedLeaveYear == personalCumulative.RemainYearLeave)
						{
							personalCumulative.RemainYearLeave -= addedLeaveYear;
							personalCumulative.IsNotificationExist = true;
							break;
						}
						addedLeaveYear -= personalCumulative.RemainYearLeave;
						personalCumulative.RemainYearLeave = 0;
						personalCumulative.IsNotificationExist = true;
					}
				}
				
				offDay.PdfUsedYearLeave = offDay.Personal.UsedYearLeave;
				offDay.PdfRemainYearLeave = offDay.Personal.TotalYearLeave - offDay.Personal.UsedYearLeave;
			} 
			//Yıllık izin değeri azalmış ise
			else if (dto.LeaveByYear < offDay.LeaveByYear)
			{
				var removeLeaveYear = offDay.LeaveByYear - dto.LeaveByYear;
				offDay.Personal.UsedYearLeave -= removeLeaveYear;
				foreach (var personalCumulative in offDay.Personal.PersonalCumulatives.OrderByDescending(pc => pc.Year))
				{
					if (personalCumulative.EarnedYearLeave != personalCumulative.RemainYearLeave)
					{
						if (personalCumulative.EarnedYearLeave - personalCumulative.RemainYearLeave > removeLeaveYear)
						{
							personalCumulative.RemainYearLeave += removeLeaveYear;
							personalCumulative.IsReportCompleted = false;
							personalCumulative.IsNotificationExist = false;
							break;
						}
						if (personalCumulative.EarnedYearLeave - personalCumulative.RemainYearLeave == removeLeaveYear)
						{
							personalCumulative.RemainYearLeave = personalCumulative.EarnedYearLeave;
							personalCumulative.IsReportCompleted = false;
							personalCumulative.IsNotificationExist = false;
							break;
						}
						else
						{
							removeLeaveYear -= personalCumulative.EarnedYearLeave - personalCumulative.RemainYearLeave;
							personalCumulative.RemainYearLeave = personalCumulative.EarnedYearLeave;
							personalCumulative.IsReportCompleted = false;
							personalCumulative.IsNotificationExist = false;
						}
					}
				}

				offDay.PdfUsedYearLeave = offDay.Personal.UsedYearLeave;
				offDay.PdfRemainYearLeave = offDay.Personal.TotalYearLeave - offDay.Personal.UsedYearLeave;
			}
			//Alacak izin değeri artmış ise
			if (dto.LeaveByTaken > offDay.LeaveByTaken)
			{
				offDay.Personal.TotalTakenLeave -= (dto.LeaveByTaken - offDay.LeaveByTaken) * 8;
				offDay.PdfRemainTakenLeave = offDay.Personal.TotalTakenLeave;
			}
			//Alacak izin değeri azalmış ise
			else if (dto.LeaveByTaken < offDay.LeaveByTaken)
			{
				offDay.Personal.TotalTakenLeave += (offDay.LeaveByTaken - dto.LeaveByTaken) * 8;
				offDay.PdfRemainTakenLeave = offDay.Personal.TotalTakenLeave;
			}
			
			if (dto.LeaveByMarriedFatherDead is not null)
			{
				dto.LeaveByMarriedFatherDead.ForEach(a =>
				{
					if (a.Contains("LeaveByFather"))
						offDay.LeaveByFather = 5;
					else
						offDay.LeaveByFather = 0;
					
					if (a.Contains("LeaveByDead"))
						offDay.LeaveByDead = 3;
					else
						offDay.LeaveByDead = 0;
					if (a.Contains("LeaveByMarried"))
						offDay.LeaveByMarried = 3;
					else
						offDay.LeaveByMarried = 0;
				});
			}
			else
			{
				offDay.LeaveByFather = 0;
				offDay.LeaveByDead = 0;
				offDay.LeaveByMarried = 0;
			}
			
			offDay.StartDate = dto.StartDate;
			offDay.EndDate = dto.EndDate;
			offDay.Description = dto.Description;
			offDay.LeaveByYear = dto.LeaveByYear;
			offDay.LeaveByTaken = dto.LeaveByTaken;
			offDay.LeaveByTravel = dto.LeaveByTravel;
			offDay.LeaveByWeek = dto.LeaveByWeek;
			offDay.LeaveByFreeDay = dto.LeaveByFreeDay;
			offDay.LeaveByPublicHoliday = dto.LeaveByPublicHoliday;
			offDay.CountLeave = dto.CountLeave;
			await _unitOfWork.WriteOffDayRepository.Update(offDay);
			
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "OffDay",
				LogType = LogType.Update,
				Description = $"{offDay.Personal.NameSurname} adlı Personele ait onaylanmış izin güncellendi.",
				IpAddress = ipAddress,
				UserID = userId,
			});
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return result;
	}

	public async Task<IResultDto> UpdateFirstWaitingStatusOffDayService(Guid id,bool status,string username,Guid userId,string ipAddress)
	{
		IResultDto result = new ResultDto();
		string logDescription = string.Empty;
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(predicate: p => 
			p.ID == id && 
			p.Status == EntityStatusEnum.Online,
			include: i=> i.Include(p=> p.Personal));
			if(offday is null) return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			offday.HrName = username;
			if (status)
			{
				if (offday.LeaveByYear > 0 && (offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave) < offday.LeaveByYear)
					return result.SetStatus(false).SetErr("Personal Year Leave Insufficient").SetMessage("Personele ait yıllık izin sayısı yetersiz.");
				offday.OffDayStatus = OffDayStatusEnum.WaitingForSecond;
				logDescription = $"{offday.Personal.NameSurname} adlı Personele ait bekleyen izin onaylandı.";
				var getDirectorMail = await _unitOfWork.ReadBranchUserRepository.GetSingleAsync(
				predicate: p => p.BranchID == offday.BranchId && p.User.Role == UserRoleEnum.Director,
				include: p => p.Include(a => a.User));
				if (getDirectorMail is null || getDirectorMail.User is null)
					return result.SetStatus(false).SetErr("Director not found").SetMessage("Personele ait genel müdür aranırken bir hata oluştu!!");
				var mailHtml = await _emailService.GetMailemplateHtml(
					"waitingoffday",
					getDirectorMail.User.Username,
					offday.Personal.NameSurname,
					offday.HrName,
					"insan kaynakları",
					DateTime.Now);
				var isMailSendSuccess = await _emailService.SendEmailAsync(getDirectorMail.User.Email, "İyaş Personel Takip Bekleyen İzin Onayı", mailHtml);
				if (isMailSendSuccess is false || string.IsNullOrEmpty(mailHtml))
					return result.SetStatus(false).SetErr("Mail Send Fail").SetMessage("Eposta Gönderilirken Bir Hata oluştu! Lütfen daha sonra tekrar deneyiniz...");
			}
			else
			{
				offday.OffDayStatus = OffDayStatusEnum.Rejected;
				logDescription = $"{offday.Personal.NameSurname} adlı Personele ait bekleyen izin reddedildi.";
			}
			
			await _unitOfWork.WriteOffDayRepository.Update(offday);
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "OffDay",
				LogType = LogType.Update,
				Description = $"{logDescription}",
				IpAddress = ipAddress,
				UserID = userId,
			});
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

	public async Task<IResultDto> UpdateSecondWaitingStatusOffDayService(Guid id, bool status,string username,Guid userId,string ipAddress)
	{
		IResultDto result = new ResultDto();
		string logDescription = string.Empty;
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => p.ID == id,
				include: p=> p.Include(a=>a.Personal).ThenInclude(b=>b.PersonalCumulatives)
				);
			if(offday is null) return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (status) // Eğer onaylanmış ise
			{
				if (offday.LeaveByYear > 0) // Eğer İzin Raporunda Yıllık İzin dolu ise
				{
					if(((offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave) < offday.LeaveByYear)) return result.SetStatus(false).SetErr("Personal Year Leave Insufficient").SetMessage("Personele ait yıllık izin sayısı yetersiz.");
					offday.Personal.UsedYearLeave += offday.LeaveByYear;
					var addedLeaveYear = offday.LeaveByYear; // Sıfırlanana kadar döngü içinde eksilt
					foreach (var personalCumulative in offday.Personal.PersonalCumulatives.OrderBy(pc => pc.Year))
					{
						if (personalCumulative.RemainYearLeave > 0)
						{
							if (addedLeaveYear < personalCumulative.RemainYearLeave)
							{
								personalCumulative.RemainYearLeave -= addedLeaveYear;
								break;
							}
							else if(addedLeaveYear == personalCumulative.RemainYearLeave)
							{
								personalCumulative.RemainYearLeave -= addedLeaveYear;
								personalCumulative.IsNotificationExist = true;
								break;
							}
							else
							{
								addedLeaveYear -= personalCumulative.RemainYearLeave;
								personalCumulative.RemainYearLeave = 0;
								personalCumulative.IsNotificationExist = true;
							}
						}
					}
					
				}
				
				if (offday.LeaveByTaken > 0)
				{
					offday.Personal.TotalTakenLeave -= (offday.LeaveByTaken * 8);
					
				}
				var getMaxDocNumber = _unitOfWork.ReadOffDayRepository
					.GetAll(predicate: p => p.OffDayStatus == OffDayStatusEnum.Approved &&
											p.Status == EntityStatusEnum.Online)
					.Max(d => (int?)d.DocumentNumber);
				if (getMaxDocNumber is null) // Döküman Numarası Bulunamazsa sıfır yap daha sonra bu 1 değerine tekabül edecek
					getMaxDocNumber = 0;

				offday.DocumentNumber = getMaxDocNumber!.Value + 1;
				offday.OffDayStatus = OffDayStatusEnum.Approved;
				logDescription = $"{offday.Personal.NameSurname} adlı Personele ait bekleyen izin onaylandı.";
			}
			else
			{
				offday.OffDayStatus = OffDayStatusEnum.Rejected;
				logDescription = $"{offday.Personal.NameSurname} adlı Personele ait bekleyen izin reddedildi.";
			}
			offday.PdfUsedYearLeave = offday.Personal.UsedYearLeave;
			offday.PdfRemainYearLeave = (offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave);
			offday.PdfRemainTakenLeave = offday.Personal.TotalTakenLeave;
			offday.DirectorName = username;
			await _unitOfWork.WriteOffDayRepository.Update(offday);
			var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.ID == userId);
			if(user is null) return result.SetStatus(false).SetErr("User Not Found").SetMessage("Oturumunuz ile ilgili bir problem olabilir. Lütfen Sisteme tekrar giriş yapınız!");
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "OffDay",
				LogType = LogType.Update,
				Description = $"{logDescription}",
				IpAddress = ipAddress,
				UserID = userId,
			});
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

	public async Task<IResultDto> DeleteOffDayService(Guid id,Guid userId,string ipAddress)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offDay = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate:p=> 
				p.ID == id && 
				p.Status == EntityStatusEnum.Online && 
				p.Personal.Status == EntityStatusEnum.Online,
				include: p=> 
				p.Include(a=> a.Personal)
				.ThenInclude(i=>i.PersonalCumulatives)
			);
			if(offDay is null)
				return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (offDay.LeaveByYear > 0)// Alınan yıllık izini geri ata
			{
				offDay.Personal.UsedYearLeave -= offDay.LeaveByYear;
				var tempRemoveLeaveYear = offDay.LeaveByYear;
				foreach (var personalCumulative in offDay.Personal.PersonalCumulatives.OrderByDescending(pc => pc.Year))
				{
					if (personalCumulative.EarnedYearLeave != personalCumulative.RemainYearLeave)
					{
						if (personalCumulative.EarnedYearLeave- personalCumulative.RemainYearLeave > tempRemoveLeaveYear)
						{
							personalCumulative.RemainYearLeave += tempRemoveLeaveYear;
							personalCumulative.IsReportCompleted = false;
							personalCumulative.IsNotificationExist = false;
							break;
						}
						if (personalCumulative.EarnedYearLeave - personalCumulative.RemainYearLeave == tempRemoveLeaveYear)
						{
							personalCumulative.RemainYearLeave = personalCumulative.EarnedYearLeave;
							personalCumulative.IsReportCompleted = false;
							personalCumulative.IsNotificationExist = false;
							break;
						}
						else
						{
							tempRemoveLeaveYear -= personalCumulative.EarnedYearLeave - personalCumulative.RemainYearLeave;
							personalCumulative.RemainYearLeave = personalCumulative.EarnedYearLeave;
							personalCumulative.IsNotificationExist = false;
							personalCumulative.IsReportCompleted = false;

						}
					}
				}
			} 
			if (offDay.LeaveByTaken > 0) // Alınan alacak iznini geri ata
				offDay.Personal.TotalTakenLeave += (offDay.LeaveByTaken * 8);
			offDay.Status = EntityStatusEnum.Deleted;
			offDay.DeletedAt = DateTime.Now;
			await _unitOfWork.WriteOffDayRepository.Update(offDay);
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "OffDay",
				LogType = LogType.Delete,
				Description = $"{offDay.Personal.NameSurname} adlı Personele ait onaylanmış izin iptal edildi.",
				IpAddress = ipAddress,
				UserID = userId,
			});
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