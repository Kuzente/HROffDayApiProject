using System.Globalization;
using Core.DTOs;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Services.Abstract.ExcelServices;

namespace Services.Concrete.ExcelServices;

public class ReadExcelServices : IReadExcelServices
{
    public async Task<IResultWithDataDto<List<AddRangePersonalDto>>> ImportDataFromExcel(IFormFile file)
    {
        IResultWithDataDto<List<AddRangePersonalDto>> result = new ResultWithDataDto<List<AddRangePersonalDto>>();
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
                return result.SetStatus(false).SetErr("File is null or empty").SetMessage("Yüklemiş Olduğunuz Dosya Bulunamadı.");
            using var stream = file.OpenReadStream();
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
                var branchIdString = worksheet.Cells[row, 1].GetValue<string>();
                if (string.IsNullOrEmpty(branchIdString))
                    return result.SetStatus(false).SetErr("BranchID is null").SetMessage($"{row} satırında Şubesi atlanmış personel var!!!");
                if (!Guid.TryParse(branchIdString, out var branchId))
                    return result.SetStatus(false).SetErr("BranchID format is not guid").SetMessage($"{row} satırında Şube Kodu geçersiz format!!!");
                personel.Branch_Id = branchId;
                var positionIdString = worksheet.Cells[row, 2].GetValue<string>();
                if (string.IsNullOrEmpty(positionIdString))
                    return result.SetStatus(false).SetErr("PositionID is null").SetMessage($"{row} satırında Ünvanı atlanmış personel var!!!");
                if (!Guid.TryParse(positionIdString, out var positionId))
                    return result.SetStatus(false).SetErr("PositionID format is not guid").SetMessage($"{row} satırında Ünvan Kodu Geçersiz Format!!!");
                personel.Position_Id = positionId;
                if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].GetValue<string>()))
                    return result.SetStatus(false).SetErr("Name is null or empty").SetMessage($"{row} satırında Ad Soyad atlanmış personel var!!!");
                personel.NameSurname = worksheet.Cells[row, 3].GetValue<string>();
                DateTime startJobDate;
                if (!DateTime.TryParseExact(worksheet.Cells[row, 4].Text, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out startJobDate))
                {
                    return result.SetStatus(false).SetErr("StartJobDate is null or empty").SetMessage($"{row} satırında İşe Başlama Tarihi atlanmış personel var!!!");
                }
                if (startJobDate.Year <= 1950)
                {
                    return result.SetStatus(false).SetErr("StartJobDate is null or empty").SetMessage($"{row} satırında İşe Başlama Tarihi yılı 1950 den küçük personel var!!!");
                }
                personel.StartJobDate = startJobDate;
                DateTime birthDate;
                if (!DateTime.TryParseExact(worksheet.Cells[row, 5].Text, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    return result.SetStatus(false).SetErr("BirthDate is null or empty").SetMessage($"{row} satırında Doğum Tarihi atlanmış personel var!!!");
                }
                if (birthDate.Year <= 1900)
                {
                    return result.SetStatus(false).SetErr("BirthDate is null or empty").SetMessage($"{row} satırında Doğum Tarihi yılı 1900 den küçük personel var!!!");
                }
                personel.BirthDate = birthDate;
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 6].GetValue<string>()))    
                    return result.SetStatus(false).SetErr("BirthPlace is null or empty").SetMessage($"{row} satırında Doğum Yeri atlanmış personel var!!!");
                personel.PersonalDetails.BirthPlace = worksheet.Cells[row, 6].GetValue<string>();
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 7].GetValue<string>()))
                    return result.SetStatus(false).SetErr("IdentificationNumber is null or empty").SetMessage($"{row} satırında TC Kimlik numarası atlanmış personel var!!!");
                personel.IdentificationNumber = worksheet.Cells[row, 7].GetValue<string>();
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 8].GetValue<string>()))
                    return result.SetStatus(false).SetErr("RegistirationNumber is null or empty").SetMessage($"{row} satırında Sicil Numarası atlanmış personel var!!!");
                personel.RegistirationNumber = worksheet.Cells[row, 8].GetValue<int>();
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 9].GetValue<string>()))
                    return result.SetStatus(false).SetErr("IdentificationNumber is null or empty").SetMessage($"{row} satırında SSK Numarası atlanmış personel var!!!");
                personel.PersonalDetails.SskNumber =  worksheet.Cells[row, 9].GetValue<string>();
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 10].GetValue<string>()))
                    return result.SetStatus(false).SetErr("SgkCode is null or empty").SetMessage($"{row} satırında SGK Kodu atlanmış personel var!!!");
                personel.PersonalDetails.SgkCode = worksheet.Cells[row, 10].GetValue<string>(); 
                personel.RetiredOrOld = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 11].GetValue<string>()); // Eğer boş ise false ata
                if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 11].GetValue<string>()))
                {
                    personel.RetiredDate = null;  
                }
                else
                {
                    if(!(worksheet.Cells[row, 12].GetValue<DateTime>().Year > 1900))
                        return result.SetStatus(false).SetErr("RetiredDate is null or empty").SetMessage($"{row} satırında Emeklilik Tarihi 1900 den küçük personel var!!!");
                    personel.RetiredDate = worksheet.Cells[row, 12].GetValue<DateTime>();
                }
                personel.PersonalDetails.Handicapped = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 13].GetValue<string>()); // Eğer boş ise false ata
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 14].GetValue<string>()))
                    return result.SetStatus(false).SetErr("Gender is null or empty").SetMessage($"{row} satırında Cinsiyeti Atlanmış Personel Var!!!");
                if(worksheet.Cells[row, 14].GetValue<string>() != "Erkek"&& worksheet.Cells[row, 14].GetValue<string>() != "Kadın")
                    return result.SetStatus(false).SetErr("Gender format is wrong").SetMessage($"{row} satırında Cinsiyet tanımlaması yanlış olan personel var!!!");
                personel.Gender = worksheet.Cells[row, 14].GetValue<string>();
                var salaryString = worksheet.Cells[row, 15].GetValue<string>();
                if (!double.TryParse(salaryString, out var salary))
                {
                    return result.SetStatus(false).SetErr("Salary format is not valid").SetMessage($"{row} satırında Maaş formatı geçersiz.");
                }
                personel.PersonalDetails.Salary = salary;
                if(string.IsNullOrWhiteSpace(worksheet.Cells[row, 16].GetValue<string>()))
                    return result.SetStatus(false).SetErr("Departmant is null or empty").SetMessage($"{row} satırında Departmanı atlanmış personel var!!!");
                personel.PersonalDetails.DepartmantName = worksheet.Cells[row, 16].GetValue<string>(); 
                personel.PersonalDetails.MotherName = worksheet.Cells[row, 17].GetValue<string>();
                personel.PersonalDetails.FatherName = worksheet.Cells[row, 18].GetValue<string>();
                personel.PersonalDetails.EducationStatus = worksheet.Cells[row, 19].GetValue<string>();
                personel.PersonalDetails.PersonalGroup = worksheet.Cells[row, 20].GetValue<string>();
                personel.Phonenumber = worksheet.Cells[row, 21].GetValue<string>();
                personel.PersonalDetails.MaritalStatus = worksheet.Cells[row, 22].GetValue<string>();
                personel.PersonalDetails.BodySize = worksheet.Cells[row, 23].GetValue<string>();
                personel.PersonalDetails.BloodGroup = worksheet.Cells[row, 24].GetValue<string>();
                personel.PersonalDetails.BankAccount = worksheet.Cells[row, 25].GetValue<string>();
                personel.PersonalDetails.IBAN = worksheet.Cells[row, 26].GetValue<string>();
                personel.PersonalDetails.Address = worksheet.Cells[row, 27].GetValue<string>();
                personel.YearLeaveDate = worksheet.Cells[row, 28].GetValue<DateTime>().Year > 1000 ? worksheet.Cells[row, 28].GetValue<DateTime>() : personel.StartJobDate;
                personel.IsYearLeaveRetired = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 29].GetValue<string>());
                personel.CumulativeFormula = string.IsNullOrWhiteSpace(worksheet.Cells[row, 30].GetValue<string>()) ? "0+" : worksheet.Cells[row, 30].GetValue<string>() + "+";
                personel.TotalYearLeave = worksheet.Cells[row, 31].GetValue<int>();
                if (personel.TotalYearLeave != personel.CumulativeFormula.Split('+').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).Sum())
                {
                    return result.SetStatus(false).SetErr("TotalYearLeave and Cumulative are not equal").SetMessage($"{row} satırında yıllık izin formülasyonu ile toplam yıllık izin miktarı uyuşmuyor!!!"); 
                }
                personel.UsedYearLeave = worksheet.Cells[row, 32].GetValue<int>();
                if (personel.UsedYearLeave > personel.TotalYearLeave)
                {
                    return result.SetStatus(false).SetErr("TotalYearLeave and Cumulative are not equal").SetMessage($"{row} satırında kullanılan yıllık izin toplam yıllık izin miktarından büyük!!!"); 
                }
                personel.TotalTakenLeave = worksheet.Cells[row, 33].GetValue<int>();
                personel.FoodAid = worksheet.Cells[row, 34].GetValue<int>();
                personel.FoodAidDate = worksheet.Cells[row, 35].GetValue<DateTime>().Year > 1000 ? worksheet.Cells[row, 35].GetValue<DateTime>() : personel.StartJobDate;
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
}