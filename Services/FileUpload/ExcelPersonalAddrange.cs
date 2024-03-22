using AutoMapper;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs;
using OfficeOpenXml;
using Core.Entities;
using Microsoft.AspNetCore.Http;
namespace Services.FileUpload;
public class ExcelPersonalAddrange
{
    
    public List<AddRangePersonalDto> ImportDataFromExcel(IFormFile file)
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    try
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("Dosya Yok veya Bulunamadı.");
        }

        using var stream = file.OpenReadStream();
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0];
        // Excel'deki verileri temsil edecek bir `List<Personal>` nesnesi oluşturalım
        //List<Personal> personelListesi = new List<Personal>();
        List<AddRangePersonalDto> personelListesiDto = new();
                
        // Excel'deki verileri `List<Personal>` nesnesine ekleyelim
                
        for (int row = 2; row < worksheet.Dimension.Rows +1 ; row++)
        {
            AddRangePersonalDto personel = new AddRangePersonalDto();
            personel.PersonalDetails = new AddRangePersonalDetailDto();
            var branchIdString = worksheet.Cells[row, 1].GetValue<string>();
            if (string.IsNullOrEmpty(branchIdString))
            {
                throw new Exception("Şubesi atlanmış personel var!!!");
            }
            Guid branchId;
            if (!Guid.TryParse(branchIdString, out branchId))
            {
                throw new Exception("Branch_Id geçersiz format!!!");
            }
            personel.Branch_Id = branchId;
            var positionIdString = worksheet.Cells[row, 2].GetValue<string>();
            if (string.IsNullOrEmpty(positionIdString))
            {
                throw new Exception("Ünvanı atlanmış personel var!!!");
            }
            Guid positionId;
            if (!Guid.TryParse(positionIdString, out positionId))
            {
                throw new Exception("Unvan_ID geçersiz format!!!");
            }
            personel.Position_Id = positionId;
            personel.NameSurname = string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].GetValue<string>()) ? throw new Exception("Adı Soyadı atlanmış personel var!!!") : worksheet.Cells[row, 6].GetValue<string>();
            personel.StartJobDate = worksheet.Cells[row, 4].GetValue<DateTime>().Year > 1000 ? worksheet.Cells[row, 5].GetValue<DateTime>() : throw new Exception("İşe Başlama Tarihi atlanmış personel var!!!");
            personel.BirthDate = worksheet.Cells[row, 5].GetValue<DateTime>().Year > 1000 ? worksheet.Cells[row, 5].GetValue<DateTime>() : throw new Exception("Doğum Tarihi atlanmış personel var!!!");
            personel.PersonalDetails.BirthPlace = string.IsNullOrWhiteSpace(worksheet.Cells[row, 6].GetValue<string>()) ? throw new Exception("Doğum Yeri atlanmış personel var!!!") : worksheet.Cells[row, 6].GetValue<string>();
            personel.IdentificationNumber = string.IsNullOrWhiteSpace(worksheet.Cells[row, 7].GetValue<string>()) ? throw new Exception("TC Kimlik numarası atlanmış personel var!!!"):worksheet.Cells[row, 7].GetValue<string>();
            personel.RegistirationNumber = worksheet.Cells[row, 8].GetValue<int>();
            personel.PersonalDetails.SskNumber = string.IsNullOrWhiteSpace(worksheet.Cells[row, 9].GetValue<string>()) ? throw new Exception("SSK Numarası atlanmış personel var!!!"):worksheet.Cells[row, 9].GetValue<string>();
            personel.PersonalDetails.SgkCode = string.IsNullOrWhiteSpace(worksheet.Cells[row, 10].GetValue<string>()) ? throw new Exception("SGK Kodu atlanmış personel var!!!") : worksheet.Cells[row, 10].GetValue<string>();
            personel.RetiredOrOld = string.IsNullOrWhiteSpace(worksheet.Cells[row, 11].GetValue<string>());
            if (!personel.RetiredOrOld)
            {
                personel.RetiredDate = worksheet.Cells[row, 12].GetValue<DateTime>().Year > 1000 ? worksheet.Cells[row, 12].GetValue<DateTime>() : throw new Exception("Emeklilik Tarihi Atlanmış Personel Var!!!");  
            }
            else
            {
                personel.RetiredDate = null;
            }
            personel.PersonalDetails.Handicapped = string.IsNullOrWhiteSpace(worksheet.Cells[row, 13].GetValue<string>());
            personel.Gender = string.IsNullOrWhiteSpace(worksheet.Cells[row, 14].GetValue<string>()) ? throw new Exception("Cinsiyeti Atlanmış Personel Var!!!") : worksheet.Cells[row, 14].GetValue<string>();
            personel.PersonalDetails.Salary = worksheet.Cells[row, 15].GetValue<double>();
            personel.PersonalDetails.DepartmantName = string.IsNullOrWhiteSpace(worksheet.Cells[row, 16].GetValue<string>()) ? throw new Exception("Departmanı atlanmış personel var!!!") : worksheet.Cells[row, 16].GetValue<string>();
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
            personel.TotalYearLeave = worksheet.Cells[row, 28].GetValue<int>();
            personel.UsedYearLeave = worksheet.Cells[row, 29].GetValue<int>();
            personel.TotalTakenLeave = worksheet.Cells[row, 30].GetValue<int>();
            personel.FoodAid = worksheet.Cells[row, 31].GetValue<int>();
            personel.FoodAidDate = worksheet.Cells[row, 32].GetValue<DateTime>().Year > 1000 ? worksheet.Cells[row, 32].GetValue<DateTime>() : personel.StartJobDate;
            personelListesiDto.Add(personel);
        }
        return personelListesiDto;
    }
    catch (Exception ex)
    {
        throw ;
    }
    
    return new List<AddRangePersonalDto>();
}

}