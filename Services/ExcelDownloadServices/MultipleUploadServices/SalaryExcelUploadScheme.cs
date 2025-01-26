using Core.DTOs.MultipleUploadDtos;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Services.ExcelDownloadServices.MultipleUploadServices;
public class SalaryExcelUploadScheme
{
	public byte[] ExportToExcel(List<SalaryUpdateDto> personals)
	{
		try
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			// Excel dosyasını oluşturun.
			FileInfo excelFile = new FileInfo("TopluMaasTaslak.xlsx");
			using (ExcelPackage package = new ExcelPackage(excelFile))
			{
				// Excel dosyasının çalışma kitabını oluşturun.
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Maaslar");
				// ... Diğer sütun başlıklarını ekleyin.



				#region personalsSection
				worksheet.Column(1).Style.Numberformat.Format = "@";
				worksheet.Column(2).Style.Numberformat.Format = "@";
				worksheet.Column(3).Style.Numberformat.Format = "@";
				worksheet.Column(4).Style.Numberformat.Format = "@";
				worksheet.Cells[1, 1].Value = "Personel Kodu";
				worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Goldenrod);
				worksheet.Cells[1, 2].Value = "Personel Adı";
				worksheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Goldenrod);
				worksheet.Cells[1, 3].Value = "Güncel Maaşı";
				worksheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Goldenrod);
				worksheet.Cells[1, 4].Value = "Yeni Maaşı";
				worksheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
				worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Goldenrod);
				int row = 2;
				foreach (var personal in personals)
				{
					worksheet.Cells[row, 1].Value = personal.Id;
					worksheet.Cells[row, 2].Value = personal.NameSurname;
					worksheet.Cells[row, 3].Value = personal.Salary;

					// ... Diğer alanları ekleyin.

					row++;
				}
				#endregion

				return package.GetAsByteArray();
			}
		}
		catch (Exception)
		{

			throw;
		}
	}
}
