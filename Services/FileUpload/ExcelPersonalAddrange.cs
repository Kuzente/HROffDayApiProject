using AutoMapper;
using Core.DTOs.PersonalDTOs;
using OfficeOpenXml;
using Core.Entities;
using Microsoft.AspNetCore.Http;
namespace Services.FileUpload;
public class ExcelPersonalAddrange
{
    private readonly IMapper _mapper;

    public ExcelPersonalAddrange(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<AddRangePersonalDto> ImportDataFromExcel(IFormFile file)
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    try
    {
        if (file == null || file.Length == 0)
        {
            // Dosya yok veya boş ise gerekli hata yönetimini burada yapabilirsiniz.
        }
        else
        {
            using (var stream = file.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                // Excel'deki verileri temsil edecek bir `List<Personal>` nesnesi oluşturalım
                List<Personal> personelListesi = new List<Personal>();
                
                // Excel'deki verileri `List<Personal>` nesnesine ekleyelim
                for (int row = 2; row < worksheet.Dimension.Rows -1 ; row++)
                {
                    Personal personel = new Personal();
                    personel.PersonalDetails = new PersonalDetails();
                    personel.Branch_Id = Guid.Parse(worksheet.Cells[row, 1].GetValue<string>());
                    personel.Position_Id = Guid.Parse(worksheet.Cells[row, 2].GetValue<string>());
                    personel.NameSurname = worksheet.Cells[row, 3].GetValue<string>();
                    personel.StartJobDate = worksheet.Cells[row, 4].GetValue<DateTime>();
                    personel.BirthDate = worksheet.Cells[row, 5].GetValue<DateTime>();
                    personel.PersonalDetails.BirthPlace = worksheet.Cells[row, 6].GetValue<string>();
                    personel.IdentificationNumber = worksheet.Cells[row, 7].GetValue<string>();
                    personel.RegistirationNumber = worksheet.Cells[row, 8].GetValue<string>();
                    personel.PersonalDetails.SskNumber = worksheet.Cells[row, 9].GetValue<string>();
                    personel.PersonalDetails.SgkCode = worksheet.Cells[row, 10].GetValue<string>();
                    personel.RetiredOrOld = worksheet.Cells[row, 11].GetValue<bool>();
                    personel.PersonalDetails.Handicapped = worksheet.Cells[row, 12].GetValue<bool>();
                    personel.Gender = worksheet.Cells[row, 13].GetValue<string>();
                    personel.PersonalDetails.Salary = worksheet.Cells[row, 14].GetValue<double>();
                    personel.PersonalDetails.MotherName = worksheet.Cells[row, 15].GetValue<string>();
                    personel.PersonalDetails.FatherName = worksheet.Cells[row, 16].GetValue<string>();
                    personel.PersonalDetails.EducationStatus = worksheet.Cells[row, 17].GetValue<string>();
                    personel.PersonalDetails.GraduatedSchool = worksheet.Cells[row, 18].GetValue<string>();
                    personel.Phonenumber = worksheet.Cells[row, 19].GetValue<string>();
                    personel.PersonalDetails.MaritalStatus = worksheet.Cells[row, 20].GetValue<string>();
                    personel.PersonalDetails.BodySize = worksheet.Cells[row, 21].GetValue<string>();
                    personel.PersonalDetails.BloodGroup = worksheet.Cells[row, 22].GetValue<string>();
                    personel.PersonalDetails.BankAccount = worksheet.Cells[row, 23].GetValue<string>();
                    personel.PersonalDetails.IBAN = worksheet.Cells[row, 24].GetValue<string>();
                    personel.PersonalDetails.Address = worksheet.Cells[row, 25].GetValue<string>();
                    
                    
                    personel.TotalYearLeave = worksheet.Cells[row, 26].GetValue<int>();
                    personel.UsedYearLeave = worksheet.Cells[row, 27].GetValue<int>();
                    personel.TotalTakenLeave = worksheet.Cells[row, 28].GetValue<int>();
                    personel.FoodAid = worksheet.Cells[row, 29].GetValue<int>();
                    personelListesi.Add(personel);
                }
                stream.Dispose();
                return _mapper.Map<List<AddRangePersonalDto>>(personelListesi);
            }
            
        }
    }
    catch (Exception ex)
    {
        throw new Exception("" + ex.Message +"\n"+""+ex.InnerException);
    }
    
    return new List<AddRangePersonalDto>();
}

}