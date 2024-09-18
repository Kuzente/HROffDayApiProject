using Core.Enums;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.MissingDayServices;
using Services.ExcelDownloadServices.MissingDayServices;
using Services.ExcelDownloadServices.TransferPersonalServices;

namespace UI.Controllers
{
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public class MissingDayController : Controller
	{
		private readonly IReadMissingDayService _readMissingDayService;
		private readonly MissingDayPersonalListExcelExport _reaDayPersonalListExcelExport;

		public MissingDayController(IReadMissingDayService readMissingDayService, MissingDayPersonalListExcelExport reaDayPersonalListExcelExport)
		{
			_readMissingDayService = readMissingDayService;
			_reaDayPersonalListExcelExport = reaDayPersonalListExcelExport;
		}

		public async Task<IActionResult> MissingDayList(MissingDayQuery query)
		{
			var result = await _readMissingDayService.GetMissingDayListService(query);
			return View(result);
		}
		[HttpPost]
		public async Task<IActionResult> ExportExcel(MissingDayQuery query, string returnUrl)
		{
			var result = await _readMissingDayService.ExcelGetPersonalMissingDayListService(query);
			if (result.IsSuccess)
			{
				byte[] excelData = _reaDayPersonalListExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
				var response = HttpContext.Response;
				response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				response.Headers.Add("Content-Disposition", "attachment; filename=EksikGunler.xlsx");
				await response.Body.WriteAsync(excelData, 0, excelData.Length);
				return new EmptyResult();
			}

			return Redirect(returnUrl);
		}
	}
}
