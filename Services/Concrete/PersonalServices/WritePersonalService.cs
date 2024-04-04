using AutoMapper;
using Core;
using Core.DTOs;
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
			mapSet.IsYearLeaveRetired = false;
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
			
            var mapSet = _mapper.Map<List<Personal>>(writeDto);
            mapSet.ForEach(p =>
            {
	            p.FoodAidDate = p.StartJobDate;
				p.YearLeaveDate = p.StartJobDate;
            });
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
			mapSet.YearLeaveDate = getPersonal.YearLeaveDate;
			mapSet.IsYearLeaveRetired = getPersonal.IsYearLeaveRetired;
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
			var data = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate:p=> p.ID == dto.ID,include:a=>a.Include(b=>b.PersonalDetails));
			if (data is null) return res.SetStatus(false).SetErr("Not Found Data").SetMessage("İlgili Veri Bulunamadı!!!");
			if (data.Status == EntityStatusEnum.Online)
			{
				// Eğer "online" ise "offline" yapın
				data.Status = EntityStatusEnum.Offline;
				data.EndJobDate = dto.EndJobDate;
			}
			else if (data.Status == EntityStatusEnum.Offline)
			{
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
				if (data.RetiredOrOld) // Eğer Eski kayıt personel emekli ise yeni kayıt üzerinde emeklilik durumu değerlendirilecek
				{
					if (dto.IsYearLeaveProtected)
					{
						newPersonel.IsYearLeaveRetired = true;
					}
					
				}
				data.IsBackToWork = true; // Personeli geri işe alındı olarak işaretle
				if (dto.IsYearLeaveProtected) // Yıllık izinleri koru seçeneği işaretlenirse
				{
					newPersonel.TotalYearLeave = data.TotalYearLeave;
					newPersonel.UsedYearLeave = data.UsedYearLeave;
				}

				if (dto.IsYearLeaveDateProtected)// yıllık izin tarihi korunuyorsa eski işe giriş tarihini yeni personelde yıllık izin yenilenme tarihi olarak ayarla
				{
					newPersonel.YearLeaveDate = data.StartJobDate; 
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
}