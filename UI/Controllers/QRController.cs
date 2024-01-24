using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;
[Authorize]
public class QrController : Controller
{
    // GET
    public IActionResult QRList()
    {
        return View();
    }
}