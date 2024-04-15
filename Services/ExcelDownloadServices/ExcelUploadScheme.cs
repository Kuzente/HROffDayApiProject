using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Services.ExcelDownloadServices;

public class ExcelUploadScheme
{
    public byte[] ExportToExcel(List<PositionNameDto> positions , List<BranchNameDto> branches)
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
            string[] headersRequired = {
                "Şube Kodu", 
                "Ünvan Kodu", 
                "Adı Soyadı", 
                "İşe Başlama Tarihi", 
                "Doğum Tarihi",
                "Doğum Yeri", 
                "Tc Kimlik Numarası", 
                "Sicil Numarası", 
                "SSK No", 
                "SGK Kodu",
                "Emekli Mi", 
                "Emeklilik Tarihi",
                "Engelli Mi", 
                "Cinsiyet", 
                "Maaş", 
                "Departman Adı",
            };
            string[] headersOptional = {
                "Anne Adı", 
                "Baba Adı",
                "Eğitim Durumu", 
                "Personelin Grubu", 
                "Telefon", 
                "Medeni Durumu",
                "Beden Ölçüleri", 
                "Kan Grubu", 
                "Banka Hesabı", 
                "IBAN Adresi", 
                "Adres",
                "Yıllık İzin Yenilenme Tarihi",
                "Yıllık İzin Yenilenirken Emeklilik Durumu Baz Alınacak Mı?",
                "Yıllık İzin Formulasyonu",
                "Toplam Yıllık İzin Miktarı", 
                "Kullanılan Yıllık İzin Miktarı",
                "Toplam Alınan İzin Miktarı (SAAT)", 
                "Gıda Yardımı (TL)",
                "Gıda Yardımı Yenilenme Tarihi"
            };

            int columnIndex = 1;
            foreach (var header in headersRequired)
            {
                worksheet.Cells[1, columnIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, columnIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Crimson);
                worksheet.Cells[1, columnIndex++].Value = header;
            }
            foreach (var header in headersOptional)
            {
                worksheet.Cells[1, columnIndex++].Value = header;
            }

            

            #endregion
            
                
            return package.GetAsByteArray();
        }
    }
}