using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class AuthenticationController : Controller
{
    // GET
    public IActionResult Login()
    {
        return View();
    }
}