using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Services.Abstract.PersonalServices;
using Services.Abstract.UserServices;
using UI.Models;

namespace UI.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IReadUserService _readUserService;
        private readonly IWritePersonalService _writePersonalService;

        public HomeController(IReadUserService readUserService, IWritePersonalService writePersonalService)
        {
            _readUserService = readUserService;
            _writePersonalService = writePersonalService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.FindFirst(ClaimTypes.Role).Value == nameof(UserRoleEnum.BranchManager))
            {
                if (!GetClientUserId().HasValue) return Redirect("/404");
                var result =  await _readUserService.GetUserBranches(GetClientUserId().Value);
                if (!result.IsSuccess) return Redirect("/404");
                
                return Redirect("/izin-olustur?id=" + result.Data.First());
            }
            
            return View();
        }
        public IActionResult ErrorPage(ErrorViewModel vm)
        {
            return View(vm);
        }
        public IActionResult AccessDeniedPage()
        {
            return View();
        }
		[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
		[HttpPost]
        public async Task<IActionResult> PostCumulativeNotification(Guid id)
        {
            if (!GetClientUserId().HasValue) return Redirect("/404");
            var result = await _writePersonalService.UpdatePersonalCumulativeNotificationAsyncService(id,GetClientUserId()!.Value,GetClientIpAddress());
            return Ok(result);
        }
        
    }
}