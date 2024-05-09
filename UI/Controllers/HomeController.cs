using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Core;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Services.Abstract.PersonalServices;
using Services.Abstract.UserServices;
using UI.Models;

namespace UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
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
                var result =  await _readUserService.GetUserBranches(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                if (!result.IsSuccess) return Redirect("/404");
                
                return Redirect("/izin-olustur?id=" + result.Data.First());
            }
            
            return View();
        }
        public IActionResult ErrorPage()
        {
            return View();
        }
        public IActionResult AccessDeniedPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostCumulativeNotification(Guid id)
        {
            var result = await _writePersonalService.UpdatePersonalCumulativeNotificationAsyncService(id);
            return Ok(result);
        }
        
    }
}