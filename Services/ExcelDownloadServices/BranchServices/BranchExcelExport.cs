using Core.DTOs.BranchDTOs;
using OfficeOpenXml;

namespace Services.ExcelDownloadServices.BranchServices;

public class BranchExcelExport
{
    public byte[] ExportToExcel(List<BranchDto> branches)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Excel dosyasını oluşturun.
            FileInfo excelFile = new FileInfo("Subeler.xlsx");
             using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                // Excel dosyasının çalışma kitabını oluşturun.
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Subeler");

                // Sütun başlıklarını ekleyin.
                worksheet.Cells[1, 1].Value = "Şube Kodu";
                worksheet.Cells[1, 2].Value = "Şube Adı";
                
                
                
                // ... Diğer sütun başlıklarını ekleyin.

                // Entity listesini Excel'e yazın.
                int row = 2;
                foreach (var entity in branches)
                {
                    worksheet.Cells[row, 1].Value = entity.ID;
                    worksheet.Cells[row, 2].Value = entity.Name;
                    
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