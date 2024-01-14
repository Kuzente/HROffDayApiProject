using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Services.ExcelDownloadServices;

public class ExcelUploadScheme
{
    public byte[] ExportToExcel(List<PositionDto> positions , List<BranchDto> branches)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Excel dosyasını oluşturun.
            FileInfo excelFile = new FileInfo("TopluVeriTaslak.xlsx");
            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                // Excel dosyasının çalışma kitabını oluşturun.
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Veri Sayfasi");
                ExcelWorksheet worksheetBranch = package.Workbook.Worksheets.Add("Subeler");
                ExcelWorksheet worksheetPosition = package.Workbook.Worksheets.Add("Unvanlar");
                // ... Diğer sütun başlıklarını ekleyin.

                #region branchSection

                worksheetBranch.Cells[1, 1].Value = "Şube Kodu";
                worksheetBranch.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetBranch.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LimeGreen);
                worksheetBranch.Cells[1, 2].Value = "Şube Adı";
                worksheetBranch.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetBranch.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LimeGreen);
                int row = 2;
                foreach (var branch in branches)
                {
                    worksheetBranch.Cells[row, 1].Value = branch.ID;
                    worksheetBranch.Cells[row, 2].Value = branch.Name;
                    
                    // ... Diğer alanları ekleyin.

                    row++;
                }

                #endregion

                #region positionSection
                worksheetPosition.Cells[1, 1].Value = "Ünvan Kodu";
                worksheetPosition.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetPosition.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Goldenrod);
                worksheetPosition.Cells[1, 2].Value = "Ünvan Adı";
                worksheetPosition.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetPosition.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Goldenrod);
                row = 2;
                foreach (var position in positions)
                {
                    worksheetPosition.Cells[row, 1].Value = position.ID;
                    worksheetPosition.Cells[row, 2].Value = position.Name;
                    
                    // ... Diğer alanları ekleyin.

                    row++;
                }
                #endregion

                #region mainSection

                // Sütun başlıklarını ekleyin.
                
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                worksheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                worksheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                
                worksheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                
                worksheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                
                worksheet.Cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                
                worksheet.Cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
               
                worksheet.Cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                
                worksheet.Cells[1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
               
                worksheet.Cells[1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
               
                worksheet.Cells[1, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
               
                worksheet.Cells[1, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
               
                worksheet.Cells[1, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
               
                worksheet.Cells[1, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                worksheet.Cells[1, 1].Value = "Şube Kodu";
                worksheet.Cells[1, 2].Value = "Ünvan Kodu";
                worksheet.Cells[1, 3].Value = "Adı Soyadı";
                worksheet.Cells[1, 4].Value = "İşe Başlama Tarihi";
                worksheet.Cells[1, 5].Value = "Doğum Tarihi";
                worksheet.Cells[1, 6].Value = "Doğum Yeri";
                worksheet.Cells[1, 7].Value = "Tc Kimlik Numarası";
                worksheet.Cells[1, 8].Value = "Sicil Numarası";
                worksheet.Cells[1, 9].Value = "SSK No";
                worksheet.Cells[1, 10].Value = "SGK Kodu";
                worksheet.Cells[1, 11].Value = "Emekli Mi";
                worksheet.Cells[1, 12].Value = "Engelli Mi";
                worksheet.Cells[1, 13].Value = "Cinsiyet";
                worksheet.Cells[1, 14].Value = "Maaş";
                worksheet.Cells[1, 15].Value = "Anne Adı";
                worksheet.Cells[1, 16].Value = "Baba Adı";
                worksheet.Cells[1, 17].Value = "Eğitim Durumu";
                worksheet.Cells[1, 18].Value = "Mezun Olduğu Okul";
                worksheet.Cells[1, 19].Value = "Telefon";
                worksheet.Cells[1, 20].Value = "Medeni Durumu";
                worksheet.Cells[1, 21].Value = "Beden Ölçüleri";
                worksheet.Cells[1, 22].Value = "Kan Grubu";
                worksheet.Cells[1, 23].Value = "Banka Hesabı";
                worksheet.Cells[1, 24].Value = "IBAN Adresi";
                worksheet.Cells[1, 25].Value = "Adres";

                #endregion
               
                row = 2;
                
                return package.GetAsByteArray();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}