using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Services.Abstract.BranchServices;
using Services.Abstract.DashboardServices;
using Services.Abstract.DetailedFilterServices;
using Services.Abstract.UserServices;
using UI.Helpers;
using UI.Models;

namespace UI.Controllers;
[Authorize]
[Route("query")]
[ApiController]
public class QueryController : ODataController
{
    private readonly IReadOdataService _readOdataService;
    private readonly IReadUserService _readUserService;
    private readonly IReadDetailedFilterService _readDetailedFilterService;
    private readonly IMapper _mapper;

    public QueryController(IMapper mapper, IReadOdataService readOdataService, IReadUserService readUserService, IReadDetailedFilterService readDetailedFilterService)
    {
        _mapper = mapper;
        _readOdataService = readOdataService;
        _readUserService = readUserService;
        _readDetailedFilterService = readDetailedFilterService;
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
    [Route("tum-subeler")]
    public async Task<IActionResult> GetAllBranches()
    {
        var result = await _readOdataService.GetAllBranchesOdataService();
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
    [Route("tum-unvanlar")]
    public async Task<IActionResult> GetAllPositions()
    {
        var result = await _readOdataService.GetAllPositionOdataService();
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
    [HttpGet]
    [EnableQuery]
    [Route("bekleyen-izinler-dashboard")]
    public async Task<IActionResult> GetWaitingOffDays()
    {
        bool directorRole = false;
        var userRole = User.FindFirst(ClaimTypes.Role).Value;
        if (!string.IsNullOrEmpty(userRole) && userRole == nameof(UserRoleEnum.Director))
        {
            directorRole = true;
            var branchesResult = await _readUserService.GetUserBranches(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty));
            if (!branchesResult.IsSuccess) return Redirect("/404");
            var result = await _readOdataService.GetWaitingOffDaysService(directorRole,branchesResult.Data);
            return Ok(result);
        }
        else
        {
            var result = await _readOdataService.GetWaitingOffDaysService(directorRole,null);
            return Ok(result);  
        }
    }
    [HttpGet]
    [EnableQuery]
    [Route("onaylanan-izinler-dashboard")]
    public async Task<IActionResult> GetApprovedOffDays()
    {
        bool directorRole = false;
        var userRole = User.FindFirst(ClaimTypes.Role).Value;
        if (!string.IsNullOrEmpty(userRole) && userRole == nameof(UserRoleEnum.Director))
        {
            directorRole = true;
            var branchesResult = await _readUserService.GetUserBranches(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty));
            if (!branchesResult.IsSuccess) return Redirect("/404");
            var result = await _readOdataService.GetApprovedDaysService(directorRole,branchesResult.Data);
            return Ok(result);
        }
        else
        {
            var result = await _readOdataService.GetApprovedDaysService(directorRole,null);
            return Ok(result);  
        }
    }
    [HttpGet]
    [EnableQuery]
	[Route("eksik-gun-dashboard")]
    public async Task<IActionResult> GetMissingDayList()
    {
        var result = await _readOdataService.GetMissingDayService();
        return Ok(result);
    }
    [HttpGet]
    [EnableQuery]
    [Route("kumulatif-bildirim-dashboard")]
    public async Task<IActionResult> GetNotificationCumulativeList()
    {
        var result = await _readOdataService.GetPersonalCumulativesService();
        return Ok(result);
    }
	[EnableQuery]
	[HttpGet]
	[Route("detayli-filtre/{entityName}")]
	public async Task<IActionResult> Get(string entityName)
	{
		var result = await _readDetailedFilterService.GetDetailedFilterOdataService(entityName);
		return Ok(result);
	}
    [EnableQuery]
	[HttpPost("detayli-filtre")]
	public async Task<IActionResult> DetayliFiltreGetWithPost([FromBody] ODataQueryParamsModel body)
	{
        IResultWithDataDto<object> result = new ResultWithDataDto<object>();
        //Sadece Kümülatif kolonları seçildiyse de hata dönmen lazım
		if (string.IsNullOrWhiteSpace(body.Select))
		{
			return Ok(result.SetStatus(false).SetErr("OData Select Query is null").SetMessage("Lütfen getirilecek kolon seçtiğinizden emin olunuz!(Backend)"));
		}
		var queryStringBuilder = new StringBuilder("?");
		if (!string.IsNullOrWhiteSpace(body.Filter))
		{
			queryStringBuilder.Append($"$filter={body.Filter}&");
		}
		if (!string.IsNullOrWhiteSpace(body.OrderBy))
		{
			queryStringBuilder.Append($"$orderby={body.OrderBy}&");
		}
		if (!string.IsNullOrWhiteSpace(body.Expand))
		{
			queryStringBuilder.Append($"$expand={body.Expand}&");
		}
		queryStringBuilder.Append($"$select={body.Select}");
		var queryString = queryStringBuilder.ToString();

		// Fake HttpRequest oluştur
		var httpContext = new DefaultHttpContext();
		httpContext.Request.QueryString = new QueryString(queryString);


		var queryContext = new ODataQueryContext(ODataModelConfiguration.GetEdmModel(), typeof(Personal), null);
		var queryOptions = new ODataQueryOptions<Personal>(queryContext, httpContext.Request);

		var queryableResult = await _readDetailedFilterService.GetDetailedFilterOdataService(nameof(Personal));
		var appliedQuery = queryOptions.ApplyTo(queryableResult);
       
        result.SetData(appliedQuery.Cast<object>().ToList());
		return Ok(result);
	}

}