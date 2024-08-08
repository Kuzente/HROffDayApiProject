using Core.Enums;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public class MissingDayController : Controller
	{
		//public IActionResult MissingDayList(MissingDayQuery query)
		//{
		//	return View();
		//}
		//[HttpPost]
		//public async Task<IActionResult> ExportExcel(TransferPersonalQuery query, string returnUrl)
		//{
		//	return Ok();
		//}
	}
}
