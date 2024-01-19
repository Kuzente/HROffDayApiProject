using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class QrController : Controller
{
    // GET
    public IActionResult QRList()
    {
        return View();
    }
}