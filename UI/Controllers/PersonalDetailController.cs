using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class PersonalDetailController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}