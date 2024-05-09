using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.PersonalCumulativeDtos.WriteDtos;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.PersonalServices;
using Services.HelperServices;

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

	public async Task<IResultDto> AddAsync(AddPersonalDto writePersonalDto)
	{
		IResultDto res = new ResultDto();
		try
		{
			var mapSet = _mapper.Map<Personal>(writePersonalDto);
			mapSet.FoodAidDate = mapSet.StartJobDate;
			mapSet.YearLeaveDate = mapSet.StartJobDate;
			var cumulativeFormula = CalculateCumulativeHelper.CalculateCumulative(mapSet.YearLeaveDate, mapSet.BirthDate,mapSet.IsYearLeaveRetired,mapSet.RetiredDate);
			mapSet.TotalYearLeave = cumulativeFormula
				.Split('+')
				.Select(s => !string.IsNullOrEmpty(s) ? int.Parse(s) : 0)
				.Sum();
			var personalCumulatives = CalculateCumulativeHelper.GetCumulativeList(cumulativeFormula, mapSet.YearLeaveDate.Year, 0);
			mapSet.PersonalCumulatives = personalCumulatives;
			var resultData = await _unitOfWork.WritePersonalRepository.AddAsync(mapSet);
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

	public async Task<IResultDto> AddRangeAsync(List<AddRangePersonalDto> writeDto)
	{
		IResultDto res = new ResultDto();
		try
		{
			if(writeDto == null || writeDto.Count <= 0)
                return res.SetStatus(false).SetErr("Not Found").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
            writeDto.ForEach(p =>
            {
	            var personalCumulatives = CalculateCumulativeHelper.GetCumulativeList(p.CumulativeFormula, p.YearLeaveDate.Year, p.UsedYearLeave);
	            p.PersonalCumulatives = personalCumulatives;
            });
            var mapSet = _mapper.Map<List<Personal>>(writeDto);
			await _unitOfWork.WritePersonalRepository.AddRangeAsync(mapSet);
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

	public async Task<IResultDto> UpdateAsync(WriteUpdatePersonalDto writeDto)
	{
		IResultDto result = new ResultDto();
		try
		{
			var getPersonal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate:p=> p.ID == writeDto.ID,include:a=>a.Include(b=>b.Branch).Include(c=>c.Position));
			if (getPersonal is  null) return result.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Personel Bulunamadı!!!");
			if(getPersonal.Status != EntityStatusEnum.Online) return result.SetStatus(false).SetErr("Personel is Not Active").SetMessage("İlgili Personel Aktif Olarak Çalışmamaktadır!!!");
			//Personel Şubesi veya Ünvanı Değişti ise nakil tablosuna ekleme yap
			if (getPersonal.Branch_Id != writeDto.Branch_Id || getPersonal.Position_Id != writeDto.Position_Id)
			{
				var getNewBranchName = await _unitOfWork.ReadBranchRepository.GetSingleAsync(predicate:p=>p.ID == writeDto.Branch_Id && p.Status != EntityStatusEnum.Archive);
				var getNewPositionName = await _unitOfWork.ReadPositionRepository.GetSingleAsync(predicate:p=>p.ID == writeDto.Position_Id && p.Status != EntityStatusEnum.Archive);
				if(getNewBranchName is null || getNewPositionName is null) return result.SetStatus(false).SetErr("Not Found Branch or Position").SetMessage("Girmiş Olduğunuz Şube veya Ünvan Eklenemedi!!!");
				var transferObject = new TransferPersonal
				{
					OldBranch = getPersonal.Branch.Name,
					OldPosition = getPersonal.Position.Name,
					NewBranch = getNewBranchName.Name,
					NewPosition = getNewPositionName.Name,
					Personal_Id = getPersonal.ID,
				};
				await _unitOfWork.WriteTransferPersonalRepository.AddAsync(transferObject);
			}
			var mapSet = _mapper.Map<Personal>(writeDto);
			mapSet.ID = getPersonal.ID;
			mapSet.CreatedAt = getPersonal.CreatedAt;
			mapSet.TotalYearLeave = getPersonal.TotalYearLeave;
			mapSet.UsedYearLeave = getPersonal.UsedYearLeave;
			mapSet.YearLeaveDate = getPersonal.YearLeaveDate;
			await _unitOfWork.WritePersonalRepository.Update(mapSet);
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
	

	public async Task<IResultDto> DeleteAsync(Guid id)
	{
		IResultDto res = new ResultDto();
		try
		{
			var findData = await _unitOfWork.ReadPersonalRepository.GetByIdAsync(id);
			var data = await findData.FirstOrDefaultAsync();
			if (data is null)  return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");;
			await _unitOfWork.WritePersonalRepository.DeleteAsync(data);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");;
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}
	
	public async Task<IResultDto> ChangeStatus(WritePersonalChangeStatusDto dto)
	{
		IResultDto res = new ResultDto();
		try
		{
			var data = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate:p=> 
					p.ID == dto.ID,
				include:a=>
					a.Include(b=>b.PersonalDetails).Include(c=> c.PersonalCumulatives));
			if (data is null) return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			if (data.Status == EntityStatusEnum.Online)
			{
				// Eğer "online" ise "offline" yapın
				data.Status = EntityStatusEnum.Offline;
				data.EndJobDate = dto.EndJobDate;
			}
			else if (data.Status == EntityStatusEnum.Offline)
			{
				if (data.EndJobDate > dto.StartJobDate) return res.SetStatus(false).SetErr("Invalid StartJobDate").SetMessage("İşe başlangıç tarihi işten çıkış tarihinden önce olamaz!");
				var newPersonel = new Personal
				{
					ID = Guid.NewGuid(),
					Branch_Id = data.Branch_Id,
					Position_Id = data.Position_Id,
					NameSurname = data.NameSurname,
					BirthDate = data.BirthDate,
					StartJobDate = dto.StartJobDate,
					EndJobDate = null,
					IdentificationNumber = data.IdentificationNumber,
					RegistirationNumber = data.RegistirationNumber,
					Phonenumber = data.Phonenumber,
					RetiredOrOld = data.RetiredOrOld,
					RetiredDate = data.RetiredDate,
					Gender = data.Gender,
					Status = EntityStatusEnum.Online,
					YearLeaveDate = dto.StartJobDate,
					IsYearLeaveRetired = data.IsYearLeaveRetired,
					PersonalDetails = new PersonalDetails
					{
						Address = data.PersonalDetails.Address,
						BankAccount = data.PersonalDetails.BankAccount,
						BirthPlace = data.PersonalDetails.BirthPlace,
						BloodGroup = data.PersonalDetails.BloodGroup,
						BodySize = data.PersonalDetails.BodySize,
						IBAN = data.PersonalDetails.IBAN,
						Handicapped = data.PersonalDetails.Handicapped,
						Salary = data.PersonalDetails.Salary,
						EducationStatus = data.PersonalDetails.EducationStatus,
						FatherName = data.PersonalDetails.FatherName,
						MotherName = data.PersonalDetails.MotherName,
						MaritalStatus = data.PersonalDetails.MaritalStatus,
						PersonalGroup = data.PersonalDetails.PersonalGroup,
						SgkCode = data.PersonalDetails.SgkCode,
						SskNumber = data.PersonalDetails.SskNumber,
						DepartmantName = data.PersonalDetails.DepartmantName
					}
					
				};
				
				data.IsBackToWork = true; //Önceki Personeli geri işe alındı olarak işaretle
				if (dto.IsYearLeaveProtected) // Yıllık izinleri koru seçeneği işaretlenirse
				{
					newPersonel.TotalYearLeave = data.TotalYearLeave;
					newPersonel.UsedYearLeave = data.UsedYearLeave;
					var oldPersonalCumulatives = new List<PersonalCumulative>();
					foreach (var oldCumulative in data.PersonalCumulatives)
					{
						oldPersonalCumulatives.Add(new PersonalCumulative
						{
							EarnedYearLeave = oldCumulative.EarnedYearLeave,
							RemainYearLeave = oldCumulative.RemainYearLeave,
							IsReportCompleted = oldCumulative.IsReportCompleted,
							IsNotificationExist = oldCumulative.IsNotificationExist,
							Year = oldCumulative.Year,
							Status = oldCumulative.Status,
							CreatedAt = oldCumulative.CreatedAt,
							ModifiedAt = oldCumulative.ModifiedAt,
							DeletedAt = oldCumulative.DeletedAt,
							Personal_Id = newPersonel.ID,
						});
					}
					newPersonel.PersonalCumulatives = oldPersonalCumulatives; // Eğer Yıllık izin sayıları korunuyorsa kümülatifi kopyala
					newPersonel.YearLeaveDate = data.YearLeaveDate;
				}
				else // Eğer Yıllık izin sayıları korunmuyor ise 
				{
					var cumulativeFormula = CalculateCumulativeHelper.CalculateCumulative(newPersonel.YearLeaveDate, newPersonel.BirthDate,newPersonel.IsYearLeaveRetired,newPersonel.RetiredDate);
					var personalCumulatives = CalculateCumulativeHelper.GetCumulativeList(cumulativeFormula, newPersonel.YearLeaveDate.Year, newPersonel.UsedYearLeave);
					newPersonel.PersonalCumulatives = personalCumulatives;
				}
				
				if (dto.IsTakenLeaveProtected) // Alacak İzinleri Koru işaretlenirse
				{
					newPersonel.TotalTakenLeave = data.TotalTakenLeave;
				}
				if (dto.IsFoodAidProtected) // Gıda yardımı miktarını koru işaretlenirse
				{
					newPersonel.FoodAid = data.FoodAid;
				}
				//Eğer Gıda yardımı girilmediyse işe başlangıç tarihini baz al
				newPersonel.FoodAidDate = dto.FoodAidDate.Year > 1000 ? dto.FoodAidDate : dto.StartJobDate;
				
				await _unitOfWork.WritePersonalRepository.AddAsync(newPersonel);
			}
			await _unitOfWork.WritePersonalRepository.Update(data);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");;
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
			var result = await _unitOfWork.WritePersonalRepository.RecoverAsync(id);
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

	public async Task<IResultDto> UpdatePersonalCumulativeAsyncService(WriteUpdateCumulativeDto dto)
	{
		IResultDto result = new ResultDto();
		try
		{
			var personal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p =>
				p.ID == dto.Personal_Id && 
				p.Status == EntityStatusEnum.Online,
				include: p=> p.Include(c=>c.PersonalCumulatives.OrderBy(pc=> pc.Year)));
			if(personal is null) return result.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Personel Bulunamadı!!!");
			// Ekleme işlemi kontrol edilirken boş guidden kontrol edilebilir
			if (!personal.PersonalCumulatives.Any(c => c.ID == dto.ID && c.Status == EntityStatusEnum.Online)) //Ekleme İşlemi
			{
				var personalCumulative = new PersonalCumulative
				{
					EarnedYearLeave = dto.EarnedYearLeave,
					RemainYearLeave = dto.RemainYearLeave,
					IsReportCompleted = dto.IsReportCompleted,
					IsNotificationExist = dto.IsNotificationExist,
					Year = DateTime.Now.Year
				};
				personal.PersonalCumulatives.Add(personalCumulative);
			}
			else
			{
				var personalCumulative = personal.PersonalCumulatives.FirstOrDefault(p=>p.ID == dto.ID);
				if(personalCumulative is null) return result.SetStatus(false).SetErr("Personal Cumulative Not Found").SetMessage("İlgili Kümülatif verisi bulunamadı!");
				personalCumulative.RemainYearLeave = dto.RemainYearLeave;
				personalCumulative.EarnedYearLeave = dto.EarnedYearLeave;
				personalCumulative.IsNotificationExist = dto.IsNotificationExist;
				personalCumulative.IsReportCompleted = dto.IsReportCompleted;
			}

			int totalGainedYearLeave = 0;
			int totalUsedYearLeave = 0;
			foreach (var pc in personal.PersonalCumulatives.OrderBy(pc=> pc.Year))
			{
				totalGainedYearLeave += pc.EarnedYearLeave;
				totalUsedYearLeave += pc.EarnedYearLeave - pc.RemainYearLeave;
			}

			personal.TotalYearLeave = totalGainedYearLeave;
			personal.UsedYearLeave = totalUsedYearLeave;
			await _unitOfWork.WritePersonalRepository.Update(personal);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");

		}
		catch (Exception ex)
		{
			result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}

	public async Task<IResultDto> UpdatePersonalCumulativeNotificationAsyncService(Guid id)
	{
		IResultDto result = new ResultDto();
		try
		{
			var personalCumulative = await _unitOfWork.ReadPersonalCumulativeRepository.GetSingleAsync(predicate: p =>
					p.ID == id && 
					p.Status == EntityStatusEnum.Online && 
					p.Personal.Status == EntityStatusEnum.Online,
					include: i=> i.Include(p=>p.Personal));
			if(personalCumulative is null) return result.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Kümülatif Rapor Bulunamadı!!!");
			if (personalCumulative.IsReportCompleted)
			{
				personalCumulative.IsNotificationExist = false;
			}
			else
			{
				personalCumulative.IsReportCompleted = true;
				personalCumulative.IsNotificationExist = false;
			}
			await _unitOfWork.WritePersonalCumulativeRepository.Update(personalCumulative);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception ex)
		{
			result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return result;
	}

	public async Task<IResultDto> DeletePersonalCumulativeAsyncService(Guid personalId, Guid cumulativeId)
	{
		IResultDto result = new ResultDto();
		try
		{
			var personal = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p =>
					p.ID == personalId && 
					p.Status == EntityStatusEnum.Online &&
					p.PersonalCumulatives.Any(c => c.ID == cumulativeId && c.Status == EntityStatusEnum.Online),
				include: p=> p.Include(c=>c.PersonalCumulatives));
			if(personal is null) return result.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Personel Bulunamadı!!!");
				var deletedCumulative = personal.PersonalCumulatives.First(p => p.ID == cumulativeId);
				personal.TotalYearLeave -= deletedCumulative.EarnedYearLeave;
				personal.UsedYearLeave -= (deletedCumulative.EarnedYearLeave - deletedCumulative.RemainYearLeave);
				await _unitOfWork.WritePersonalCumulativeRepository.RemoveAsync(deletedCumulative);
				await _unitOfWork.WritePersonalRepository.Update(personal);
				var resultCommit = _unitOfWork.Commit();
				if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
			
		}
		catch (Exception ex)
		{
			result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return result;
	}
}