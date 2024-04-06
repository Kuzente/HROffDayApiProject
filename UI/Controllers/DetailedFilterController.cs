using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class DetailedFilterController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}