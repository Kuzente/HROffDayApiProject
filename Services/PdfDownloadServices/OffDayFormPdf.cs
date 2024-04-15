using System.Globalization;
using Core.DTOs.OffDayDTOs.ReadDtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Services.PdfDownloadServices;

public class OffDayFormPdf
{
    private ReadApprovedOffDayFormExcelExportDto Dto { get; set; }
    
    public byte[] GetOffDayPdfDocument(ReadApprovedOffDayFormExcelExportDto dto)
    {
        Dto = dto;
        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
            });
        }).GeneratePdf();
        return pdf;
    }
    private void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(12).SemiBold().FontFamily("Calibri");
        byte[] imageData = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","logonew.png"));
        container.Background(Colors.White).Height(90).Row(row =>
        {
            row.ConstantItem(90).Height(90).Image(imageData);
            row.RelativeItem().AlignMiddle().AlignCenter().Column(column =>
            {
                column.Item().PaddingRight(90).Text("IYAŞ ISPARTA GIDA SANAYİ VE TİCARET A.Ş.").Style(titleStyle);
                column.Item().PaddingRight(90).AlignCenter().Text("Yıllık İzin Formu").Style(titleStyle);
            });
        });
    }
       private void ComposeContent(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(12).SemiBold().FontFamily("Calibri");
            container.Background(Colors.White).Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem(9).PaddingTop(10).Text("Sayı: " + Dto.DocumentNumber).Style(titleStyle);
                    row.RelativeItem(3).AlignRight().PaddingTop(10).Text("Tarih: " + Dto.CreatedAt.ToString("dd MMMM yyyy",new CultureInfo("tr-TR"))).Style(titleStyle);

                });
                column.Item().Row(row =>
                {
                    row.RelativeItem(2).Text("İzin Almak İsteyen Personelin;").Style(titleStyle);
                });
                column.Item().BorderTop(3).BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Adı Soyadı:").Style(titleStyle).FontSize(10);
                    
                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.Personal.NameSurname).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("TC Kimlik No:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.Personal.IdentificationNumber).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Personel Ünvanı:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.PositionName).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Görev Yeri:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.BranchName).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Kurum Sicil Numarası:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.Personal.RegistirationNumber.ToString()).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Kullanmak İstediği İznin Şekli:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(3).Background(Colors.Red.Accent1).BorderRight(1).AlignMiddle().PaddingLeft(10).Text("Ücretli İzin").Style(titleStyle).FontSize(10);
                    row.RelativeItem(1).Background(Colors.White).BorderRight(1).AlignMiddle().AlignCenter().Text(Dto.LeaveByYear > 0 || Dto.LeaveByDead > 0 || Dto.LeaveByFather > 0 || Dto.LeaveByMarried > 0 || Dto.LeaveByTaken > 0 || Dto.LeaveByTravel > 0 || Dto.LeaveByWeek > 0 || Dto.LeaveByPublicHoliday > 0 ? "X" : "").Style(titleStyle).FontSize(15);
                    
                    row.RelativeItem(3).Background(Colors.Green.Accent1).BorderRight(1).AlignMiddle().PaddingLeft(10).Text("Ücretsiz İzin").Style(titleStyle).FontSize(10);
                    row.RelativeItem(1).Background(Colors.White).AlignMiddle().AlignCenter().Text(Dto.LeaveByFreeDay > 0 ? "X" : "").Style(titleStyle).FontSize(15);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Kullanmak İstediği İznin İçeriği:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).PaddingRight(10).Text(GetIzınCesitleriString(Dto)).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Kullanmak İstediği İznin Süresi:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().AlignCenter().PaddingLeft(10).Text($"Toplam {Dto.CountLeave.ToString()} Gün").Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Kalan İzin Süresi:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(4).Background(Colors.White).BorderRight(1).AlignMiddle().PaddingLeft(10).Text($"Kullanılan Yıllık İzin {Dto.PdfUsedYearLeave.ToString()}").Style(titleStyle).FontSize(10);
                    row.RelativeItem(4).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text($"Kalan Yıllık İzin {Dto.PdfRemainYearLeave.ToString()}").Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("Kalan Alacak İzin Süresi:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).PaddingRight(10).Text((Dto.PdfRemainTakenLeave + " Saat").ToString()).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("İznin Başlama Tarihi:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.StartDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR"))).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("İznin Bitiş Tarihi:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.EndDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR"))).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).Height(24).AlignMiddle().PaddingLeft(10).Text("İşe Başlama Tarihi:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.EndDate.AddDays(1).ToString("dd MMMM yyyy", new CultureInfo("tr-TR"))).Style(titleStyle).FontSize(10);
                });
                column.Item().BorderLeft(3).BorderRight(3).BorderBottom(1).Height(60).Row(row =>
                {
                    row.RelativeItem(4).Background("#DBE4F0").BorderRight(1).AlignMiddle().PaddingLeft(10).Text("İzin Açıklaması:").Style(titleStyle).FontSize(10);

                    row.RelativeItem(8).Background(Colors.White).AlignMiddle().PaddingLeft(10).Text(Dto.Description).Style(titleStyle).FontSize(10);
                });
                column.Item().PaddingBottom(2).BorderLeft(3).BorderRight(3).BorderBottom(3).Height(60).Row(row =>
                {
                    row.RelativeItem(9).PaddingLeft(10).PaddingTop(5).Text("Yukarıda belirtilen tarihler arasında izinli sayılmam için gereğini bilgilerinize arz ederim.").Style(titleStyle).FontSize(10);

                    row.RelativeItem(3).AlignCenter().PaddingTop(10).Text("İmza").Style(titleStyle);
                });
                column.Item().Background(Colors.White).Height(30).Row(row =>
                {
                    row.RelativeItem(12).AlignCenter().AlignMiddle().PaddingTop(10).Text("Adı geçenin yukarıda belirtilen tarihlerde izinli sayılma talebini görüşlerinize arz ederim..").Style(titleStyle).FontSize(10);
                });
                column.Item().Background(Colors.White).Height(75).Row(row =>
                {
                    row.RelativeItem().AlignMiddle().AlignCenter().PaddingTop(10).Column(col=>
                    {
                        col.Item().AlignCenter().Text(Dto.HrName).Style(titleStyle).FontSize(10);
                        col.Item().AlignCenter().Text("İnsan Kaynakları").Style(titleStyle).FontSize(9);
                    });
                    row.RelativeItem().AlignMiddle().AlignCenter().PaddingTop(10).Column(col =>
                    {
                        col.Item().AlignCenter().Text("............................").Style(titleStyle).FontSize(10);
                        col.Item().AlignCenter().Text("Bölüm Müdürü").Style(titleStyle).FontSize(9);
                    });
                });
                column.Item().AlignMiddle().Background(Colors.White).PaddingTop(10).Height(45).Row(row =>
                {
                    row.RelativeItem(12).AlignCenter().Column(col =>
                    {
                        col.Item().AlignCenter().Text("ONAYLANDI").Style(titleStyle).FontSize(10);
                        col.Item().AlignCenter().Text(Dto.DirectorName).Style(titleStyle).FontSize(10);
                        col.Item().AlignCenter().Text("Mağazalar Genel Müdürü").Style(titleStyle).FontSize(9);

                    });
                });
            });
        }
        private string GetIzınCesitleriString(ReadApprovedOffDayFormExcelExportDto dto)
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
        //private string SaatleriGunVeSaatlereCevir(double saat)
        //{
        //    // Toplam saatleri gün ve saatlere çevir
        //    int gun = (int)(saat / 8);
        //    double kalanSaat = saat % 8;

        //    // Sonucu döndür
        //    return gun + " gün " + kalanSaat.ToString("0.0") + " saat";
        //}
}