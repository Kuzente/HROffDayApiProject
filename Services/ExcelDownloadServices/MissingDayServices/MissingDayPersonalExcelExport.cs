using System.Globalization;
using Core.DTOs.MissingDayDtos.ReadDtos;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using OfficeOpenXml;

namespace Services.ExcelDownloadServices.MissingDayServices;

public class MissingDayPersonalExcelExport
{
    public byte[] ExportToExcel(List<ReadMissingDayDto> datas)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        // Excel dosyasını oluşturun.
        FileInfo excelFile = new FileInfo($"{datas.First().NameSurname}-NakilListesi.xlsx");
        using (ExcelPackage package = new ExcelPackage(excelFile))
        {
            // Excel dosyasının çalışma kitabını oluşturun.
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Nakil Listesi");

            // Sütun başlıklarını ekleyin.
            worksheet.Cells[1, 1].Value = "Adı Soyadı";
            worksheet.Cells[1, 2].Value = "Tc Kimlik No";
            worksheet.Cells[1, 3].Value = "Şube";
            worksheet.Cells[1, 4].Value = "İzin Başlangıç Tarihi";
            worksheet.Cells[1, 5].Value = "İzin Bitiş Tarihi";
            worksheet.Cells[1, 6].Value = "İşe Başlama Tarihi";
            worksheet.Cells[1, 7].Value = "Sebebi";
            worksheet.Cells[1, 8].Value = "Oluşturulma Tarihi";
                
                
            // ... Diğer sütun başlıklarını ekleyin.

            // Entity listesini Excel'e yazın.
            int row = 2;
            foreach (var entity in datas)
            {
                worksheet.Cells[row, 1].Value = entity.NameSurname;
                worksheet.Cells[row, 2].Value = entity.IdentificationNumber;
                worksheet.Cells[row, 3].Value = entity.BranchName;
                worksheet.Cells[row, 4].Value = entity.StartOffdayDate.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));;
                worksheet.Cells[row, 5].Value = entity.EndOffDayDate.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));;
                worksheet.Cells[row, 6].Value = entity.StartJobDate.HasValue ? entity.StartJobDate.Value.ToString("dd.MM.yyyy", new CultureInfo("tr-TR")) : "Yok";
                worksheet.Cells[row, 7].Value = entity.Reason;
                worksheet.Cells[row, 8].Value = entity.CreatedAt.ToString("dd.MM.yyyy", new CultureInfo("tr-TR"));
                // ... Diğer alanları ekleyin.

                row++;
            }
            return package.GetAsByteArray();
        }
    }
}