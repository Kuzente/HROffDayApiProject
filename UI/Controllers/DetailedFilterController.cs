using System.Text.Json;
using System.Text.Json.Serialization;
using Core.DTOs.DetailFilterDtos.ReadDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;
[AllowAnonymous]
public class DetailedFilterController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> FilteredResultPost(ReadDetailFilterDto dto)
    {
        //var dto = JsonSerializer.Deserialize<ReadDetailFilterDto>(jsonData);
        return Ok();
    }
}