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
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    Personal personel = new Personal();

            
                    personel.NameSurname = worksheet.Cells[row, 1].GetValue<string>();
                    personel.Branch_Id = worksheet.Cells[row, 2].GetValue<int>();
                    personel.RegistirationNumber = worksheet.Cells[row, 3].GetValue<string>();
                    personel.Position_Id = worksheet.Cells[row, 4].GetValue<int>();
                    personel.StartJobDate = worksheet.Cells[row, 5].GetValue<DateTime>();
                    personel.Gender = worksheet.Cells[row, 6].GetValue<string>();
                    personel.BirthDate = worksheet.Cells[row, 7].GetValue<DateTime>();
                    personel.IdentificationNumber = worksheet.Cells[row, 8].GetValue<string>();
                    personel.TotalYearLeave = worksheet.Cells[row, 9].GetValue<int>();
                    personel.UsedYearLeave = worksheet.Cells[row, 10].GetValue<int>();
                    personel.RetiredOrOld = worksheet.Cells[row, 11].GetValue<bool>();
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