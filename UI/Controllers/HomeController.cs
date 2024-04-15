using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Services.Abstract.UserServices;
using UI.Models;

namespace UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IReadUserService _readUserService;

        public HomeController(IReadUserService readUserService)
        {
            _readUserService = readUserService;
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
       
        
    }
}