using System.Globalization;
using Core.DTOs.PassivePersonalDtos;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Services.ExcelDownloadServices.PersonalServices;

public class PassivePersonalExcelExport
{
    public byte[] ExportToExcel(List<PassivePersonalDto> personals)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        // Excel dosyasını oluşturun.
        FileInfo excelFile = new FileInfo("CikarilanPersoneller.xlsx");

        using (ExcelPackage package = new ExcelPackage(excelFile))
        {
            // Excel dosyasının çalışma kitabını oluşturun.
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("CikarilanPersoneller");

            // Sütun başlıklarını ekleyin.
            worksheet.Cells[1, 1].Value = "Sicil No";
            worksheet.Cells[1, 2].Value = "Adı-Soyadı";
            worksheet.Cells[1, 3].Value = "Şube";
            worksheet.Cells[1, 4].Value = "Ünvan";
            worksheet.Cells[1, 5].Value = "Cinsiyet";
            worksheet.Cells[1, 6].Value = "İşe Giriş Tarihi";
            worksheet.Cells[1, 7].Value = "İşten Çıkarılma Tarihi";
            worksheet.Cells[1, 8].Value = "SGK Durum";
            worksheet.Cells[1, 9].Value = "Doğum Tarihi";
            worksheet.Cells[1, 10].Value = "Toplam Yıllık İzin";
            worksheet.Cells[1, 11].Value = "Kullandığı Yıllık İzin";
            worksheet.Cells[1, 12].Value = "Kalan Yıllık İzin";
                
                
            // ... Diğer sütun başlıklarını ekleyin.

            // Entity listesini Excel'e yazın.
            int row = 2;
            foreach (var entity in personals)
            {
               
                worksheet.Cells[row, 1].Value = entity.RegistirationNumber.ToString();
                worksheet.Cells[row, 2].Value = entity.NameSurname;
                worksheet.Cells[row, 3].Value = entity.Branch.Name;
                worksheet.Cells[row, 4].Value = entity.Position.Name;
                worksheet.Cells[row, 5].Value = entity.Gender;
                worksheet.Cells[row, 6].Value = entity.StartJobDate.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));
                worksheet.Cells[row, 7].Value = entity.EndJobDate?.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));
                worksheet.Cells[row, 8].Value = entity.RetiredOrOld ? "Emekli" : "Emekli Değil";
				worksheet.Cells[row, 9].Value = entity.BirthDate.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));
				worksheet.Cells[row, 10].Value = entity.TotalYearLeave.ToString();
                worksheet.Cells[row, 11].Value = entity.UsedYearLeave.ToString();
                worksheet.Cells[row, 12].Value = (entity.TotalYearLeave - entity.UsedYearLeave).ToString();
                if (entity.IsBackToWork)
                {
                    worksheet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Row(row).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    worksheet.Cells[row, 2].Value += "(İşe Geri Alındı)";
                }
                // ... Diğer alanları ekleyin.

                row++;
            }
            return package.GetAsByteArray();
        }
    }
}