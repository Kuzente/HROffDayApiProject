using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services.Abstract.BranchServices;

namespace UI.Controllers;
[Route("query")]
[ApiController]
public class QueryController : ODataController
{
    private readonly IReadBranchService _readBranchService;
    private readonly IMapper _mapper;

    public QueryController(IReadBranchService readBranchService, IMapper mapper)
    {
        _readBranchService = readBranchService;
        _mapper = mapper;
    }
    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }
    [HttpGet]
    [EnableQuery]
    [Route("sube-listesi")]
    public async Task<IActionResult> Get()
    {
        var result = await _readBranchService.GetBranchesOdataService();
        var mappedResult = _mapper.Map<List<BranchDto>>(result);
        mappedResult.ForEach(a=> a.Count = mappedResult.Count);
        return Ok(mappedResult);
    }
}