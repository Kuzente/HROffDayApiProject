using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using Core.Attributes;
using Core.DTOs.DetailFilterDtos.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.DetailedFilterServices;

namespace UI.Controllers;
//[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
[AllowAnonymous]
public class DetailedFilterController : Controller
{
    private readonly IReadDetailedFilterService _readDetailedFilterService;

    public DetailedFilterController(IReadDetailedFilterService readDetailedFilterService)
    {
        _readDetailedFilterService = readDetailedFilterService;
    }

    // GET
    public IActionResult Index()
    {
       
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> FilteredResultPost(ReadDetailFilterDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid) return Ok(result.SetStatus(false).SetErr("Not Valid").SetMessage("Filtrenin boş olmadığından emin olunuz!"));
        
        if (!dto.IsValid(out var errors)) return Ok(result.SetStatus(false).SetErr("Not Valid").SetMessage(errors.First()));
        await _readDetailedFilterService.GetFilteredResultService(dto);
        return Ok(result);
    }
    
}
