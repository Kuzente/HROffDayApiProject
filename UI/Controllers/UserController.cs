using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult UsersList()
    {
        return View();
    }
}