using Azure;
using Core.DTOs.PersonalDTOs;
using Core.Entities;
using OfficeOpenXml;

namespace Services.ExcelDownloadServices.PersonalServices;

public class PersonalExcelExport
{
    public byte[] ExportToExcel(List<PersonalDto> personals)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Excel dosyasını oluşturun.
            FileInfo excelFile = new FileInfo("Personeller.xlsx");

            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                // Excel dosyasının çalışma kitabını oluşturun.
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Personeller");

                // Sütun başlıklarını ekleyin.
                worksheet.Cells[1, 1].Value = "Sicil No";
                worksheet.Cells[1, 2].Value = "Adı-Soyadı";
                worksheet.Cells[1, 3].Value = "Şube";
                worksheet.Cells[1, 4].Value = "Ünvan";
                worksheet.Cells[1, 5].Value = "Cinsiyet";
                worksheet.Cells[1, 6].Value = "İşe Başlama Tarihi";
                worksheet.Cells[1, 7].Value = "Emeklilik Durumu";
                worksheet.Cells[1, 8].Value = "Toplam Yıllık İzin";
                worksheet.Cells[1, 9].Value = "Kullandığı Yıllık İzin";
                worksheet.Cells[1, 10].Value = "Kalan Yıllık İzin";
                worksheet.Cells[1, 11].Value = "Personel SQL ID";
                
                
                // ... Diğer sütun başlıklarını ekleyin.

                // Entity listesini Excel'e yazın.
                int row = 2;
                foreach (var entity in personals)
                {
                    worksheet.Cells[row, 1].Value = entity.RegistirationNumber;
                    worksheet.Cells[row, 2].Value = entity.NameSurname;
                    worksheet.Cells[row, 3].Value = entity.Branch.Name;
                    worksheet.Cells[row, 4].Value = entity.Position.Name;
                    worksheet.Cells[row, 5].Value = entity.Gender;
                    worksheet.Cells[row, 6].Value = entity.StartJobDate.ToString();
                    worksheet.Cells[row, 7].Value = entity.RetiredOrOld ? "Emekli" : "Emekli Değil";
                    worksheet.Cells[row, 8].Value = entity.TotalYearLeave.ToString();
                    worksheet.Cells[row, 9].Value = entity.UsedYearLeave.ToString();
                    worksheet.Cells[row, 10].Value = (entity.TotalYearLeave - entity.UsedYearLeave).ToString();
                    worksheet.Cells[row, 11].Value = entity.ID.ToString();
                    // ... Diğer alanları ekleyin.

                    row++;
                }
                return package.GetAsByteArray();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}