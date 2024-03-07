using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services.Abstract.BranchServices;
using Services.Abstract.DashboardServices;

namespace UI.Controllers;
[Authorize]
[Route("query")]
[ApiController]
public class QueryController : ODataController
{
    private readonly IReadOdataService _readOdataService;
    private readonly IMapper _mapper;

    public QueryController(IMapper mapper, IReadOdataService readOdataService)
    {
        _mapper = mapper;
        _readOdataService = readOdataService;
    }
    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }
    [HttpGet]
    [EnableQuery]
    [Route("sube-sayisi")]
    public async Task<IActionResult> GetBranchCount()
    {
        var result = await _readOdataService.GetBranchesOdataService();
        //mappedResult.ForEach(a=> a.Count = mappedResult.Count);
        return Ok(result);
    }
    [HttpGet]
    [EnableQuery]
    [Route("unvan-sayisi")]
    public async Task<IActionResult> GetPositionCount()
    {
        var result = await _readOdataService.GetPositionOdataService();
        return Ok(result);
    }
    [HttpGet]
    [EnableQuery]
    [Route("personel-sayisi")]
    public async Task<IActionResult> GetPersonalCount()
    {
        var result = await _readOdataService.GetPersonalOdataService();
        return Ok(result);
    }
}