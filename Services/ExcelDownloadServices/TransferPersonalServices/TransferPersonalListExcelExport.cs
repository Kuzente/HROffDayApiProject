using Core.DTOs.TransferPersonalDtos.ReadDtos;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExcelDownloadServices.TransferPersonalServices
{
	public class TransferPersonalListExcelExport
	{
		public byte[] ExportToExcel(List<ReadTransferPersonalDto> datas)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			// Excel dosyasını oluşturun.
			FileInfo excelFile = new FileInfo("NakilListesi.xlsx");
			using (ExcelPackage package = new ExcelPackage(excelFile))
			{
				// Excel dosyasının çalışma kitabını oluşturun.
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Nakil Listesi");

				// Sütun başlıklarını ekleyin.
				worksheet.Cells[1, 1].Value = "Adı Soyadı";
				worksheet.Cells[1, 2].Value = "Eski Şube";
				worksheet.Cells[1, 3].Value = "Yeni Şube";
				worksheet.Cells[1, 4].Value = "Eski Ünvan";
				worksheet.Cells[1, 5].Value = "Yeni Ünvan";
				worksheet.Cells[1, 6].Value = "Nakil Tarihi";


				// ... Diğer sütun başlıklarını ekleyin.

				// Entity listesini Excel'e yazın.
				int row = 2;
				foreach (var entity in datas)
				{
					worksheet.Cells[row, 1].Value = entity.PersonalNameSurname;
					worksheet.Cells[row, 2].Value = entity.OldBranch;
					worksheet.Cells[row, 3].Value = entity.NewBranch;
					worksheet.Cells[row, 4].Value = entity.OldPosition;
					worksheet.Cells[row, 5].Value = entity.NewPosition;
					worksheet.Cells[row, 6].Value = entity.CreatedAt.ToString("dd MMMM yyyy", new CultureInfo("tr-TR"));

					// ... Diğer alanları ekleyin.

					row++;
				}
				return package.GetAsByteArray();
			}
		}
	}
}
