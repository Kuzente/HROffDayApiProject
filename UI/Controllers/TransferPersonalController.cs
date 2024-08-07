using Core.Enums;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.TransferPersonalService;
using Services.ExcelDownloadServices.BranchServices;
using Services.ExcelDownloadServices.TransferPersonalServices;

namespace UI.Controllers
{
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public class TransferPersonalController : Controller
	{
		private readonly IReadTransferPersonalService _readTransferPersonalService;
		private readonly TransferPersonalListExcelExport _transferPersonalListExcelExport;

		public TransferPersonalController(IReadTransferPersonalService readTransferPersonalService, TransferPersonalListExcelExport transferPersonalListExcelExport)
		{
			_readTransferPersonalService = readTransferPersonalService;
			_transferPersonalListExcelExport = transferPersonalListExcelExport;
		}

		public async Task<IActionResult> TransferPersonalList(TransferPersonalQuery query)
		{
			var result = await _readTransferPersonalService.GetTransferPersonalListService(query);
			return View(result);
		}
		/// <summary>
		/// Görevlendirmeler Listesi Excel Alma Post Metodu
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> ExportExcel(TransferPersonalQuery query, string returnUrl)
		{

			var result = await _readTransferPersonalService.ExcelGetTransferPersonalListService(query);
			if (result.IsSuccess)
			{
				byte[] excelData = _transferPersonalListExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
				var response = HttpContext.Response;
				response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				response.Headers.Add("Content-Disposition", "attachment; filename=Gorevlendirmeler.xlsx");
				await response.Body.WriteAsync(excelData, 0, excelData.Length);
				return new EmptyResult();
				// _toastNotification.AddSuccessToastMessage("Başarılı", new ToastrOptions { Title = "Başarılı" });
			}

			// _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
			return Redirect(returnUrl);
		}
	}
}
