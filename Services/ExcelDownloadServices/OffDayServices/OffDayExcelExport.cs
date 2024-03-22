using System.Globalization;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.Enums;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Services.ExcelDownloadServices.OffDayServices;

public class OffDayExcelExport
{
    public byte[] ExportToExcel(List<ReadApprovedOffDayListDto> offDays)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        // Excel dosyasını oluşturun.
        FileInfo excelFile = new FileInfo("Izinler.xlsx");
        using (ExcelPackage package = new ExcelPackage(excelFile))
        {
            // Excel dosyasının çalışma kitabını oluşturun.
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Izinler");

            // Sütun başlıklarını ekleyin.
            worksheet.Cells[1, 1].Value = "Personel Adı- Soyadı";
            worksheet.Cells[1, 2].Value = "Şube";
            worksheet.Cells[1, 3].Value = "Ünvan";
            worksheet.Cells[1, 4].Value = "İzin Başlangıç Tarihi";
            worksheet.Cells[1, 5].Value = "İzin Bitiş Tarihi";
            worksheet.Cells[1, 6].Value = "Toplam Alınan İzin Gün";
            worksheet.Cells[1, 7].Value = "Yıllık İzin";
            worksheet.Cells[1, 8].Value = "Haftalık İzin";
            worksheet.Cells[1, 9].Value = "Alacak İzin";
            worksheet.Cells[1, 10].Value = "Resmi İzin";
            worksheet.Cells[1, 11].Value = "Ücretsiz İzin";
            worksheet.Cells[1, 12].Value = "Seyahat İzin";
            worksheet.Cells[1, 13].Value = "Evlenme İzin";
            worksheet.Cells[1, 14].Value = "Babalık İzin";
            worksheet.Cells[1, 15].Value = "Ölüm İzin";
            worksheet.Cells[1, 16].Value = "İzin Açıklaması";
            worksheet.Cells[1, 17].Value = "Form Oluşturulma Tarihi";
            worksheet.Cells[1, 18].Value = "Durumu";

            int row = 2;
            foreach (var entity in offDays)
            {
                worksheet.Cells[row, 1].Value = entity.Personal.NameSurname;
                worksheet.Cells[row, 2].Value = entity.BranchName;
                worksheet.Cells[row, 3].Value = entity.PositionName;
                worksheet.Cells[row, 4].Value = entity.StartDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR"));
                worksheet.Cells[row, 5].Value = entity.EndDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR"));
                worksheet.Cells[row, 6].Value = entity.CountLeave;
                worksheet.Cells[row, 7].Value = entity.LeaveByYear;
                worksheet.Cells[row, 8].Value = entity.LeaveByWeek;
                worksheet.Cells[row, 9].Value = entity.LeaveByTaken;
                worksheet.Cells[row, 10].Value = entity.LeaveByPublicHoliday;
                worksheet.Cells[row, 11].Value = entity.LeaveByFreeDay;
                worksheet.Cells[row, 12].Value = entity.LeaveByTravel;
                worksheet.Cells[row, 13].Value = entity.LeaveByMarried;
                worksheet.Cells[row, 14].Value = entity.LeaveByFather;
                worksheet.Cells[row, 15].Value = entity.LeaveByDead;
                worksheet.Cells[row, 16].Value = entity.Description;
                worksheet.Cells[row, 17].Value = entity.CreatedAt.ToString("dd MMMM yyyy HH:mm", new CultureInfo("tr-TR"));
                if (entity.Personal.Status == EntityStatusEnum.Offline)
                {
                    worksheet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Row(row).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                }
                if (entity.Personal.IsBackToWork)
                {
                    worksheet.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Row(row).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    worksheet.Cells[row, 1].Value += "(Eski Kayıt)";
                }
                worksheet.Cells[row, 18].Value = entity.OffDayStatus == OffDayStatusEnum.Approved ? "Onaylandı" : "Reddedildi";
                worksheet.Cells[row, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 18].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                

                row++;
            }

            return package.GetAsByteArray();
        }
    }
}