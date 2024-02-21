using Core.DTOs.PositionDTOs;
using OfficeOpenXml;

namespace Services.ExcelDownloadServices.PositionServices;

public class PositionExcelExport
{
    public byte[] ExportToExcel(List<PositionDto> positions)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        // Excel dosyasını oluşturun.
        FileInfo excelFile = new FileInfo("Unvanlar.xlsx");
        using (ExcelPackage package = new ExcelPackage(excelFile))
        {
            // Excel dosyasının çalışma kitabını oluşturun.
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Unvanlar");

            // Sütun başlıklarını ekleyin.
            worksheet.Cells[1, 1].Value = "Sql ID";
            worksheet.Cells[1, 2].Value = "Ünvan Adı";
                
                
                
            // ... Diğer sütun başlıklarını ekleyin.

            // Entity listesini Excel'e yazın.
            int row = 2;
            foreach (var entity in positions)
            {
                worksheet.Cells[row, 1].Value = entity.ID;
                worksheet.Cells[row, 2].Value = entity.Name;
                    
                // ... Diğer alanları ekleyin.

                row++;
            }
            return package.GetAsByteArray();
        }
    }
}