using Core.DTOs.BranchDTOs.ReadDtos;
using Core.DTOs.PositionDTOs;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExcelDownloadServices.PersonalCountsServices
{
    public class PersonalCountExcelExport
	{
		public byte[] ExportToExcel(List<DepartmentCountDto> branches)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			// Excel dosyasını oluşturun.
			FileInfo excelFile = new FileInfo("NormKadro.xlsx");
			using (ExcelPackage package = new ExcelPackage(excelFile))
			{
                
					// Excel dosyasının çalışma kitabını oluşturun.
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Norm Kadro");

					// Sütun başlıklarını ekleyin.
					worksheet.Cells[1, 1].Value = "Departman Adı";
					worksheet.Cells[1, 2].Value = "Aktif Çalışan Sayısı";



					// ... Diğer sütun başlıklarını ekleyin.

					// Entity listesini Excel'e yazın.
					int row = 2;
				int totalCount = 0;
					foreach (var dept in branches)
					{
						worksheet.Cells[row, 1].Value = dept.DepartmentName;
						worksheet.Cells[row, 2].Value = dept.Count;

					// ... Diğer alanları ekleyin.
					totalCount += dept.Count;
						row++;
					}
				worksheet.Cells[row+1, 2].Value = totalCount;
				return package.GetAsByteArray();
			}
		}
	}
}
