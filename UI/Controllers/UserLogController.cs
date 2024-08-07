using Core.Enums;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.UserLogServices;

namespace UI.Controllers;
[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class UserLogController : Controller
{
    private readonly IReadUserLogService _readUserLogService;

	public UserLogController(IReadUserLogService readUserLogService)
	{
		_readUserLogService = readUserLogService;
	}

	// GET
	public async Task<IActionResult> UsersLogList(UserLogQuery query)
    {
		var result = await _readUserLogService.GetUsersLogsListService(query);
        return View(result);
    }
}