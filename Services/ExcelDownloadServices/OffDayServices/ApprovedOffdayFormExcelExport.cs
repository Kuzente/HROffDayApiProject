using System.Globalization;
using Core.DTOs.OffDayDTOs.ReadDtos;
using OfficeOpenXml;

namespace Services.ExcelDownloadServices.OffDayServices;

public class ApprovedOffdayFormExcelExport
{
    public byte[] ExportToExcel(ReadApprovedOffDayFormExcelExportDto dto, string path)
    {
        byte[] updatedFile;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                // Çalışma sayfasını seç
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Değiştirilecek metinleri belirle
                string formNumber = "{1}";
                string formCreatedAt = "{2}";
                string formNameSurname = "{3}";
                string formTcKimlik = "{4}";
                string formUnvanAdi = "{5}";
                string formSubeAdi = "{6}";
                string formSicilNo = "{7}";
                string formUcretliTik = "{8}";
                string formUcretsizTik = "{9}";
                string formIzinCesitleri = "{10}";
                string formToplamAlinanGun = "{11}";
                string formKullanılanİzin = "{12}";
                string formKalanİzin = "{13}";
                string formIzinBaslangic = "{16}";
                string formIzinBitis = "{17}";
                string formIseBaslamaTarihi = "{18}";
                string formIzinAciklama = "{19}";
                

                // Excel içerisinde belirtilen hücredeki metinleri değiştir
                ReplaceTextInWorksheet(worksheet, formNumber, "Rastgele Numara");
                ReplaceTextInWorksheet(worksheet, formCreatedAt,
                    dto.CreatedAt.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")));
                ReplaceTextInWorksheet(worksheet, formNameSurname, dto.Personal.NameSurname);
                ReplaceTextInWorksheet(worksheet, formTcKimlik, dto.Personal.IdentificationNumber);
                ReplaceTextInWorksheet(worksheet, formUnvanAdi, dto.Personal.Position.Name);
                ReplaceTextInWorksheet(worksheet, formSubeAdi, dto.Personal.Branch.Name);
                ReplaceTextInWorksheet(worksheet, formSicilNo, dto.Personal.RegistirationNumber);
                ReplaceTextInWorksheet(worksheet, formIzinBaslangic,
                    dto.StartDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")));
                ReplaceTextInWorksheet(worksheet, formIzinBitis,
                    dto.EndDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")));
                ReplaceTextInWorksheet(worksheet, formIseBaslamaTarihi,
                    dto.EndDate.AddDays(1).ToString("dd MMMM yyyy", new CultureInfo("tr-TR")));
                ReplaceTextInWorksheet(worksheet, formIzinAciklama, dto.Description);
                ReplaceTextInWorksheet(worksheet, formUcretsizTik, dto.LeaveByFreeDay > 0 ? "X" : "");
                ReplaceTextInWorksheet(worksheet, formUcretliTik, dto.LeaveByYear > 0 || dto.LeaveByDead > 0 || dto.LeaveByFather > 0 || dto.LeaveByMarried > 0 || dto.LeaveByTaken > 0 || dto.LeaveByTravel > 0 || dto.LeaveByWeek > 0 || dto.LeaveByPublicHoliday > 0 ? "X" : "");
                ReplaceTextInWorksheet(worksheet, formIzinCesitleri, getIzınCesitleriString(dto));
                ReplaceTextInWorksheet(worksheet, formToplamAlinanGun, dto.CountLeave.ToString());
                ReplaceTextInWorksheet(worksheet, formToplamAlinanGun, dto.CountLeave.ToString());
                ReplaceTextInWorksheet(worksheet, formKullanılanİzin, dto.Personal.UsedYearLeave.ToString());
                ReplaceTextInWorksheet(worksheet, formKalanİzin, (dto.Personal.TotalYearLeave - dto.Personal.UsedYearLeave).ToString());
                
                package.SaveAs(memoryStream);

                // MemoryStream kullanarak byte dizisine çevir
                // using (MemoryStream stream = new MemoryStream())
                // {
                //     package.SaveAs(stream);
                //     return stream.ToArray();
                // }
            }
            updatedFile = memoryStream.ToArray();
        }

        return updatedFile;
    }

    private string getIzınCesitleriString(ReadApprovedOffDayFormExcelExportDto dto)
    {
        string metin = "";
        if (dto.LeaveByYear > 0)       metin += $"{dto.LeaveByYear} Gün Yıllık İzin + ";
        if (dto.LeaveByFreeDay > 0)    metin += $"{dto.LeaveByFreeDay} Gün Ücretsiz İzin + ";
        if (dto.LeaveByDead > 0)       metin += $"{dto.LeaveByDead} Gün Ölüm İzni + ";
        if (dto.LeaveByFather > 0)     metin += $"{dto.LeaveByFather} Gün Babalık İzni + ";
        if (dto.LeaveByTaken > 0)      metin += $"{dto.LeaveByTaken} Gün Alacak İzni + ";
        if (dto.LeaveByMarried > 0)    metin += $"{dto.LeaveByMarried} Gün Evlilik İzni + ";
        if (dto.LeaveByTravel > 0)     metin += $"{dto.LeaveByTravel} Gün Seyahat İzni + ";
        if (dto.LeaveByWeek > 0)       metin += $"{dto.LeaveByWeek} Gün Haftalık İzin + ";
        if (dto.LeaveByPublicHoliday > 0) metin += $"{dto.LeaveByPublicHoliday} Gün Resmi İzin + ";
        // Eğer metinde son karakter + işareti ise onu kaldır
        if (!string.IsNullOrEmpty(metin) && metin.EndsWith(" + "))
        {
            metin = metin.Substring(0, metin.Length - 3);
        }
        return metin;
    }
    private void ReplaceTextInWorksheet(ExcelWorksheet worksheet, string searchText, string replaceText)
    {
        foreach (var cell in worksheet.Cells)
        {
            if (cell.Text.Contains(searchText))
            {
                cell.Value = cell.Text.Replace(searchText, replaceText);
            }
        }
    }
}