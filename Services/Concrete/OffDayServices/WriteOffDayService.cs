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

	public async Task<IResultDto> UpdateApprovedOffDayService(WriteUpdateWatingOffDayDto dto)
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
					else if (a.Contains("LeaveByDead"))
						offDay.LeaveByDead = 3;
					else if (a.Contains("LeaveByMarried"))
						offDay.LeaveByMarried = 3;
				});
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

	public async Task<IResultDto> UpdateFirstWaitingStatusOffDayService(Guid id,bool status,string username)
	{
		IResultDto result = new ResultDto();
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(predicate: p => 
			p.ID == id && 
			p.Status == EntityStatusEnum.Online,
			include: i=> i.Include(p=> p.Personal));
			if(offday is null) return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (status)
			{
				if (offday.LeaveByYear > 0 && (offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave) < offday.LeaveByYear)
					return result.SetStatus(false).SetErr("Personal Year Leave Insufficient").SetMessage("Personele ait yıllık izin sayısı yetersiz.");
				offday.OffDayStatus = OffDayStatusEnum.WaitingForSecond;
			}
			else
			{
				offday.OffDayStatus = OffDayStatusEnum.Rejected;
			}
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
				include: p=> p.Include(a=>a.Personal).ThenInclude(b=>b.PersonalCumulatives)
				);
			if(offday is null) return result.SetStatus(false).SetErr("OffDay Is Not Found").SetMessage("İlgili İzin Bulunamadı.");
			if (status) // Eğer onaylanmış ise
			{
				if (offday.LeaveByYear > 0 && !((offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave) < offday.LeaveByYear)) // Eğer İzin Raporunda Yıllık İzin dolu ise
				{
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
				else
					return result.SetStatus(false).SetErr("Personal Year Leave Insufficient").SetMessage("Personele ait yıllık izin sayısı yetersiz.");

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
			}
			else
			{
				offday.OffDayStatus = OffDayStatusEnum.Rejected;
			}
			offday.PdfUsedYearLeave = offday.Personal.UsedYearLeave;
			offday.PdfRemainYearLeave = (offday.Personal.TotalYearLeave - offday.Personal.UsedYearLeave);
			offday.PdfRemainTakenLeave = offday.Personal.TotalTakenLeave;
			offday.DirectorName = username;
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