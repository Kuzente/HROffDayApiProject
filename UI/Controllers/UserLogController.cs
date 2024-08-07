using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class UserLogController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}