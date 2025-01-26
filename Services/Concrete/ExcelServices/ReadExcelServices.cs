using System.Collections.Generic;
using System.Globalization;
using Core.DTOs;
using Core.DTOs.MultipleUploadDtos;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Services.Abstract.ExcelServices;

namespace Services.Concrete.ExcelServices;

public class ReadExcelServices : IReadExcelServices
{
	

	public async Task<IResultWithDataDto<List<AddRangePersonalDto>>> ImportPersonalUploadDataFromExcel(IFormFile file)
    {
        IResultWithDataDto<List<AddRangePersonalDto>> result = new ResultWithDataDto<List<AddRangePersonalDto>>();
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
                return result.SetStatus(false).SetErr("File is null or empty").SetMessage("Yüklemiş Olduğunuz Dosya Bulunamadı.");
			using var stream = new MemoryStream();
			await file.CopyToAsync(stream);
			stream.Position = 0;
			using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            List<AddRangePersonalDto> personelListesiDto = new();
            var deneme = worksheet.Dimension.Rows;
            for (int row = 2; row < worksheet.Dimension.Rows +1 ; row++)
            {
                var personel = new AddRangePersonalDto
                {
                    PersonalDetails = new AddRangePersonalDetailDto()
                };
				#region REQUIRED FIELDS
					#region branchID
						var branchIdString = worksheet.Cells[row, 1].GetValue<string>();
						if (string.IsNullOrEmpty(branchIdString))
							return result.SetStatus(false).SetErr("BranchID is null").SetMessage($"{row} satırında Şubesi atlanmış personel var!!!");
						if (!Guid.TryParse(branchIdString, out var branchId))
							return result.SetStatus(false).SetErr("BranchID format is not guid").SetMessage($"{row} satırında Şube Kodu geçersiz format!!!");
						personel.Branch_Id = branchId;
					#endregion
					#region positionID
						var positionIdString = worksheet.Cells[row, 2].GetValue<string>();
						if (string.IsNullOrEmpty(positionIdString))
							return result.SetStatus(false).SetErr("PositionID is null").SetMessage($"{row} satırında Ünvanı atlanmış personel var!!!");
						if (!Guid.TryParse(positionIdString, out var positionId))
							return result.SetStatus(false).SetErr("PositionID format is not guid").SetMessage($"{row} satırında Ünvan Kodu Geçersiz Format!!!");
						personel.Position_Id = positionId;
					#endregion
					#region nameSurname
						string nameSurname = worksheet.Cells[row, 3].GetValue<string>();
						if (string.IsNullOrWhiteSpace(nameSurname))
							return result.SetStatus(false).SetErr("Name is null or empty").SetMessage($"{row} satırında Ad Soyad atlanmış personel var!!!");
						personel.NameSurname = nameSurname;
					#endregion
					#region startJobDate
						if (!DateTime.TryParseExact(worksheet.Cells[row, 4].Text, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startJobDate))
						{
							return result.SetStatus(false).SetErr("StartJobDate is null or empty").SetMessage($"{row} satırında İşe Başlama Tarihi atlanmış personel var!!!");
						}
						if (startJobDate.Year <= 1950)
						{
							return result.SetStatus(false).SetErr("StartJobDate is null or empty").SetMessage($"{row} satırında İşe Başlama Tarihi yılı 1950 den küçük personel		var!!!");
						}
						personel.StartJobDate = startJobDate;
					#endregion
					#region birtDate
						if (!DateTime.TryParseExact(worksheet.Cells[row, 5].Text, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate))
						{
							return result.SetStatus(false).SetErr("BirthDate is null or empty").SetMessage($"{row} satırında Doğum Tarihi atlanmış personel var!!!");
						}
						if (birthDate.Year <= 1900)
						{
							return result.SetStatus(false).SetErr("BirthDate is lesser than 1900").SetMessage($"{row} satırında Doğum Tarihi yılı 1900 den küçük personel var!!!");
						}
						personel.BirthDate = birthDate;
					#endregion
					#region birthPlace
						var birthPlace = worksheet.Cells[row, 6].GetValue<string>();
						if (string.IsNullOrWhiteSpace(birthPlace))
							return result.SetStatus(false).SetErr("BirthPlace is null or empty").SetMessage($"{row} satırında Doğum Yeri atlanmış personel var!!!");
						personel.PersonalDetails.BirthPlace = birthPlace;
					#endregion
					#region identificationNumber
						var identificationNumber = worksheet.Cells[row, 7].GetValue<string>();
						if (string.IsNullOrWhiteSpace(identificationNumber))
							return result.SetStatus(false).SetErr("IdentificationNumber is null or empty").SetMessage($"{row} satırında TC Kimlik numarası atlanmış personel var!!!");
						if (identificationNumber.Length != 11)
							return result.SetStatus(false).SetErr("IdentificationNumber length is not equal 11").SetMessage($"{row} satırında TC Kimlik numarası 11 haneli değil!!!");
						personel.IdentificationNumber = identificationNumber;
				#endregion
					#region registirationNumber
						var registirationNumberString = worksheet.Cells[row, 8].GetValue<string>();
						if(string.IsNullOrWhiteSpace(registirationNumberString))
							return result.SetStatus(false).SetErr("RegistirationNumber is null or empty").SetMessage($"{row} satırında Sicil Numarası atlanmış personel var!!!");
						if (!int.TryParse(registirationNumberString, out var registirationNumber) || registirationNumber < 0)
							return result.SetStatus(false).SetErr("RegistirationNumber format is not valid").SetMessage($"{row} satırında Sicil Numarası formatı geçersiz.");
						personel.RegistirationNumber = registirationNumber;
					#endregion
					#region sskNumber
						var sskNumber = worksheet.Cells[row, 9].GetValue<string>();
						if (string.IsNullOrWhiteSpace(sskNumber))
							return result.SetStatus(false).SetErr("SskNumber is null or empty").SetMessage($"{row} satırında SSK Numarası atlanmış personel var!!!");
						personel.PersonalDetails.SskNumber = sskNumber;
					#endregion
					#region sgkCode
						var sgkCode = worksheet.Cells[row, 10].GetValue<string>();
						if (string.IsNullOrWhiteSpace(sgkCode))
							return result.SetStatus(false).SetErr("SgkCode is null or empty").SetMessage($"{row} satırında SGK Kodu atlanmış personel var!!!");
						personel.PersonalDetails.SgkCode = sgkCode;
					#endregion
					#region retiredOrOld & retiredDate
						personel.RetiredOrOld = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 11].GetValue<string>()); // Eğer boş ise false ata
						if (!personel.RetiredOrOld)
						{
							personel.RetiredDate = null;
						}
						else
						{
							if (!DateTime.TryParseExact(worksheet.Cells[row, 12].Text, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime retiredDate))
								return result.SetStatus(false).SetErr("RetiredDate is null or empty").SetMessage($"{row} satırında Emeklilik Tarihi atlanmış personel var!!!");
							if (retiredDate.Year <= 1900)
								return result.SetStatus(false).SetErr("RetiredDate is null or empty").SetMessage($"{row} satırında Emeklilik Tarihi yılı 1900 den küçük personel	var!!!");
							personel.RetiredDate = retiredDate;
						}
					#endregion
					#region handicapped
						personel.PersonalDetails.Handicapped = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 13].GetValue<string>()); // Eğer boş ise false ata
					#endregion
					#region gender
						var gender = worksheet.Cells[row, 14].GetValue<string>();
						if (string.IsNullOrWhiteSpace(gender))
							return result.SetStatus(false).SetErr("Gender is null or empty").SetMessage($"{row} satırında Cinsiyeti Atlanmış Personel Var!!!");
						if (gender != "Erkek" && gender != "Kadın")
							return result.SetStatus(false).SetErr("Gender format is wrong").SetMessage($"{row} satırında Cinsiyet tanımlaması yanlış olan personel var!!!");
						personel.Gender = gender;
					#endregion
					#region salary
						var salaryString = worksheet.Cells[row, 15].GetValue<string>();
						if (!double.TryParse(salaryString, out var salary))
							return result.SetStatus(false).SetErr("Salary format is not valid").SetMessage($"{row} satırında Maaş formatı geçersiz.");
						personel.PersonalDetails.Salary = salary;
					#endregion
					#region departmantName
						var departmantName = worksheet.Cells[row, 16].GetValue<string>();
						if (string.IsNullOrWhiteSpace(departmantName))
							return result.SetStatus(false).SetErr("Departmant is null or empty").SetMessage($"{row} satırında Departmanı atlanmış personel var!!!");
						personel.PersonalDetails.DepartmantName = departmantName;
					#endregion
				#endregion

				#region OPTIONAL FIELDS
					#region motherName & fatherName
						personel.PersonalDetails.MotherName = worksheet.Cells[row, 17].GetValue<string>();
						personel.PersonalDetails.FatherName = worksheet.Cells[row, 18].GetValue<string>();
				#endregion
					#region educationStatus & personalGroup
						personel.PersonalDetails.EducationStatus = worksheet.Cells[row, 19].GetValue<string>();
						personel.PersonalDetails.PersonalGroup = worksheet.Cells[row, 20].GetValue<string>();
				#endregion
					#region phoneNumber
						var phoneNumber = worksheet.Cells[row, 21].GetValue<string>();
						if (phoneNumber is not null && phoneNumber.Length != 10)
							return result.SetStatus(false).SetErr("PhoneNumber lenght must be 10.").SetMessage($"{row} satırında telefon numarası 10 karakter olacak şekilde	düzenleyiniz.		Örn: 5XXXXXXXXX");
						personel.Phonenumber = phoneNumber;
					#endregion
					#region maritalStatus & bodySize & bloodGroup & bankAccount
						personel.PersonalDetails.MaritalStatus = worksheet.Cells[row, 22].GetValue<string>();
						personel.PersonalDetails.BodySize = worksheet.Cells[row, 23].GetValue<string>();
						personel.PersonalDetails.BloodGroup = worksheet.Cells[row, 24].GetValue<string>();
						personel.PersonalDetails.BankAccount = worksheet.Cells[row, 25].GetValue<string>();
					#endregion
					#region iban
						var iban = worksheet.Cells[row, 26].GetValue<string>();
						if (!string.IsNullOrWhiteSpace(iban) && iban.Length != 24)
							return result.SetStatus(false).SetErr("IBAN lenght must be 24.").SetMessage($"{row} satırında IBAN 24 karaktere eşit değil. IBAN tanımlarken 'TR' ekini				kullanmayınız.");
						personel.PersonalDetails.IBAN = iban;
					#endregion
					#region address
						personel.PersonalDetails.Address = worksheet.Cells[row, 27].GetValue<string>();
				#endregion
					#region yearLeaveDate
						if (!DateTime.TryParseExact(worksheet.Cells[row, 28].Text, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime yearLeaveDate))
							personel.YearLeaveDate = personel.StartJobDate;
						else
						{
							if (yearLeaveDate.Year < 1900)
								return result.SetStatus(false).SetErr("YearLeaveDate is lesser than 1900").SetMessage($"{row} satırında yıllık izin yenilenme tarihi yılı 1900 den küçük		personel var!!!");
							personel.YearLeaveDate = yearLeaveDate;
						}
					#endregion
					#region isYearLeaveRetired & cumulativeFormula
						personel.IsYearLeaveRetired = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 29].GetValue<string>());
						personel.CumulativeFormula = string.IsNullOrWhiteSpace(worksheet.Cells[row, 30].GetValue<string>()) ? "0+" : worksheet.Cells[row, 30].GetValue<string>() + "+";
					#endregion

					#region totalYearLeave
						var totalYearLeaveString = worksheet.Cells[row, 31].GetValue<string>();
						if (string.IsNullOrWhiteSpace(totalYearLeaveString))
						{
							personel.TotalYearLeave = 0;
						}
						else
						{
							if (!int.TryParse(totalYearLeaveString, out var totalYearLeave) || totalYearLeave < 0)
								return result.SetStatus(false).SetErr("TotalYearLeave format is not valid").SetMessage($"{row} satırında toplam yıllık izin miktar formatı geçersiz.");
							personel.TotalYearLeave = totalYearLeave;
						}
						if (personel.TotalYearLeave != personel.CumulativeFormula.Split('+').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).Sum())
						{
							return result.SetStatus(false).SetErr("TotalYearLeave and Cumulative are not equal").SetMessage($"{row} satırında yıllık izin formülasyonu ile toplam	yıllık		izin    miktarı uyuşmuyor!!!");
						}

				#endregion
					#region usedYearLeave
						var usedYearLeaveString = worksheet.Cells[row, 32].GetValue<string>();
						if (string.IsNullOrWhiteSpace(usedYearLeaveString))
						{
							personel.UsedYearLeave = 0;
						}
						else
						{
							if (!int.TryParse(usedYearLeaveString, out var usedYearLeave) || usedYearLeave < 0)
								return result.SetStatus(false).SetErr("UsedYearLeave format is not valid").SetMessage($"{row} satırında kullanılan yıllık izin miktar formatı	geçersiz.");
							personel.UsedYearLeave = usedYearLeave;
						}
						
						if (personel.UsedYearLeave > personel.TotalYearLeave)
							return result.SetStatus(false).SetErr("UsedYearLeave is bigger than TotalYearLeave.").SetMessage($"{row} satırında kullanılan yıllık izin toplam yıllık izin	miktarından büyük!!!");
					#endregion

					#region totalTakenYearLeave
						var totalTakenLeaveString = worksheet.Cells[row, 33].GetValue<string>();
						if (string.IsNullOrWhiteSpace(totalTakenLeaveString))
						{
							personel.TotalTakenLeave = 0;
						}
						else
						{
							if (!double.TryParse(totalTakenLeaveString, out var totaltakenleave))
								return result.SetStatus(false).SetErr("Total Taken Leave format is not valid").SetMessage($"{row} satırında Toplam Alacak İzin formatı geçersiz.");
							personel.TotalTakenLeave = totaltakenleave;
						}
				#endregion

					#region foodAid
						var foodAidString = worksheet.Cells[row, 34].GetValue<string>();
						if (string.IsNullOrWhiteSpace(foodAidString))
						{
							personel.FoodAid = 0;
						}
						else
						{
							if (!int.TryParse(foodAidString, out var foodAid) || foodAid < 0)
								return result.SetStatus(false).SetErr("FoodAid format is not valid").SetMessage($"{row} satırında gıda yardım formatı geçersiz.");
							personel.FoodAid = foodAid;
						}
				#endregion
					#region foodAidDate
				var foodAidDateString = worksheet.Cells[row, 35].GetValue<string>();
						if (string.IsNullOrWhiteSpace(foodAidDateString))
							personel.FoodAidDate = personel.StartJobDate;
						else
						{
							if (!DateTime.TryParseExact(foodAidDateString, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime foodAidDate))
								return result.SetStatus(false).SetErr("FoodAidDate format is not valid").SetMessage($"{row} satırında gıda yardımı yenilenme tarihi yılı geçersiz	formatta!!!");
							if (foodAidDate.Year < 1900)
								return result.SetStatus(false).SetErr("FoodAidDate is lesser than 1900").SetMessage($"{row} satırında gıda yardımı yenilenme tarihi yılı 1900 den küçük personel var!!!");
							personel.FoodAidDate = foodAidDate;
						}
					#endregion
				#endregion


				personelListesiDto.Add(personel);
            }
            if (personelListesiDto.Count <= 0)
                return result.SetStatus(false).SetErr("Personal Count wrong").SetMessage("Excel Boş olamaz!!!");
            result.SetData(personelListesiDto);

        }
        catch (Exception ex)
        {
            return result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }
        return result;
    }

	public async Task<IResultWithDataDto<List<SalaryUpdateDto>>> ImportSalaryUploadDataFromExcel(IFormFile file)
	{
		IResultWithDataDto<List<SalaryUpdateDto>> result = new ResultWithDataDto<List<SalaryUpdateDto>>();
		List<SalaryUpdateDto> salaryUpdates = new List<SalaryUpdateDto>();
		try
        {
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			if (file == null || file.Length == 0)
				return result.SetStatus(false).SetErr("File is null or empty").SetMessage("Yüklemiş Olduğunuz Dosya Bulunamadı.");

            using var stream = new MemoryStream();
            //using var stream = file.OpenReadStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
			using var package = new ExcelPackage(stream);
			var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if(worksheet is null)
				return result.SetStatus(false).SetErr("Worksheet not found").SetMessage("Excel dosyasındaki sayfa bulunamadı.");

			var rowCount = worksheet.Dimension.Rows;
			for (int row = 2; row <= rowCount; row++)
			{
				var personalIdString = worksheet.Cells[row, 1].GetValue<string>();
				var salaryString = worksheet.Cells[row, 4].GetValue<string>();

				// Validate PersonalId
				if (string.IsNullOrEmpty(personalIdString))
					return result.SetStatus(false)
								 .SetErr("PersonalID is null")
								 .SetMessage($"{row} satırında Personal idsi atlanmış personel var!");
				
				if (!Guid.TryParse(personalIdString, out var personalId))
					return result.SetStatus(false)
								 .SetErr("PersonalID format is not GUID")
								 .SetMessage($"{row} satırında Personal id kodu geçersiz format!");
				// Validate Salary
				if (string.IsNullOrEmpty(salaryString) || !double.TryParse(salaryString, out var salary) || salary < 0)
					return result.SetStatus(false)
								 .SetErr("Invalid salary")
								 .SetMessage($"{row} satırında geçersiz maaş değeri!");
				
				salaryUpdates.Add(new SalaryUpdateDto
				{
					Id = personalId,
					NewSalary = salary
				});

			}
            result.SetData(salaryUpdates);
		}
		catch (Exception ex)
		{
			return result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return result;
	}
	public async Task<IResultWithDataDto<List<IbanUpdateDto>>> ImportIbanUploadDataFromExcel(IFormFile file)
	{
		IResultWithDataDto<List<IbanUpdateDto>> result = new ResultWithDataDto<List<IbanUpdateDto>>();
		List<IbanUpdateDto> ibanUpdates = new List<IbanUpdateDto>();
		try
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			if (file == null || file.Length == 0)
				return result.SetStatus(false).SetErr("File is null or empty").SetMessage("Yüklemiş Olduğunuz Dosya Bulunamadı.");

			using var stream = new MemoryStream();
			await file.CopyToAsync(stream);
			stream.Position = 0;
			using var package = new ExcelPackage(stream);
			var worksheet = package.Workbook.Worksheets.FirstOrDefault();

			if (worksheet is null)
				return result.SetStatus(false).SetErr("Worksheet not found").SetMessage("Excel dosyasındaki sayfa bulunamadı.");

			var rowCount = worksheet.Dimension.Rows;
			for (int row = 2; row <= rowCount; row++)
			{
				var personalIdString = worksheet.Cells[row, 1].GetValue<string>();
				var ibanString = worksheet.Cells[row, 4].GetValue<string>();

				// Validate PersonalId
				if (string.IsNullOrEmpty(personalIdString))
					return result.SetStatus(false)
								 .SetErr("PersonalID is null")
								 .SetMessage($"{row} satırında Personal idsi atlanmış personel var!");

				if (!Guid.TryParse(personalIdString, out var personalId))
					return result.SetStatus(false)
								 .SetErr("PersonalID format is not GUID")
								 .SetMessage($"{row} satırında Personal id kodu geçersiz format!");
				// Validate IBAN
				if (!string.IsNullOrEmpty(ibanString) && ibanString.Length != 24)
					return result.SetStatus(false)
						.SetErr("IBAN lenght must be 24.")
						.SetMessage("IBAN 24 karaktere eşit değil. IBAN tanımlarken 'TR' ekini	kullanmayınız.");

				ibanUpdates.Add(new IbanUpdateDto
				{
					Id = personalId,
					NewIBAN = ibanString
				});

			}
			result.SetData(ibanUpdates);
		}
		catch (Exception ex)
		{
			return result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return result;
	}
	public async Task<IResultWithDataDto<List<BankAccountUpdateDto>>> ImportBankAccountUploadDataFromExcel(IFormFile file)
	{
		IResultWithDataDto<List<BankAccountUpdateDto>> result = new ResultWithDataDto<List<BankAccountUpdateDto>>();
		List<BankAccountUpdateDto> bankAccountUpdates = new List<BankAccountUpdateDto>();
		try
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			if (file == null || file.Length == 0)
				return result.SetStatus(false).SetErr("File is null or empty").SetMessage("Yüklemiş Olduğunuz Dosya Bulunamadı.");

			using var stream = new MemoryStream();
			await file.CopyToAsync(stream);
			stream.Position = 0;
			using var package = new ExcelPackage(stream);
			var worksheet = package.Workbook.Worksheets.FirstOrDefault();

			if (worksheet is null)
				return result.SetStatus(false).SetErr("Worksheet not found").SetMessage("Excel dosyasındaki sayfa bulunamadı.");

			var rowCount = worksheet.Dimension.Rows;
			for (int row = 2; row <= rowCount; row++)
			{
				var personalIdString = worksheet.Cells[row, 1].GetValue<string>();
				var bankAccountString = worksheet.Cells[row, 4].GetValue<string>();

				// Validate PersonalId
				if (string.IsNullOrEmpty(personalIdString))
					return result.SetStatus(false)
								 .SetErr("PersonalID is null")
								 .SetMessage($"{row} satırında Personal idsi atlanmış personel var!");

				if (!Guid.TryParse(personalIdString, out var personalId))
					return result.SetStatus(false)
								 .SetErr("PersonalID format is not GUID")
								 .SetMessage($"{row} satırında Personal id kodu geçersiz format!");
				

				bankAccountUpdates.Add(new BankAccountUpdateDto
				{
					Id = personalId,
					NewBankAccount = bankAccountString
				});

			}
			result.SetData(bankAccountUpdates);
		}
		catch (Exception ex)
		{
			return result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return result;
	}
}