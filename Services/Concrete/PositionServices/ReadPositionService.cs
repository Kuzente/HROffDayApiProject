using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Services.Abstract.PositionServices;

namespace Services.Concrete.PositionServices;

public class ReadPositionService : IReadPositionService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadPositionService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<bool> GetAnyByNameAsync(string name)
	{
		var result = await _unitOfWork.ReadPositionRepository.GetAny(p=> p.Name == name);
		return result;
	}

	public async Task<IResultWithDataDto<List<PositionDto>>> GetExcelPositionListService(PositionQuery query)
	{
		IResultWithDataDto<List<PositionDto>> res = new ResultWithDataDto<List<PositionDto>>();
		try
		{
			var resultData = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll(
				orderBy: p=> p.OrderBy(a=>a.Name),
				predicate: p=> (p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline) &&
				               (string.IsNullOrWhiteSpace(query.search) || p.Name.Contains(query.search))&&
                               (!query.active.HasValue || (query.active.Value && p.Status == EntityStatusEnum.Online)) &&
                                (!query.passive.HasValue || (query.passive.Value && p.Status == EntityStatusEnum.Offline))
            ));
			var mapData = _mapper.Map<List<PositionDto>>(resultData.ToList());
			res.SetData(mapData);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<ResultWithPagingDataDto<List<PositionDto>>> GetPositionListService(PositionQuery query)
    {
        ResultWithPagingDataDto<List<PositionDto>> res = new ResultWithPagingDataDto<List<PositionDto>>(query.sayfa, query.search);
        try
        {
            var allData = await Task.Run(() =>
            _unitOfWork.ReadPositionRepository.GetAll(
                orderBy: p => p.OrderBy(a => a.Name),
                predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
                                (string.IsNullOrEmpty(query.search) || a.Name.Contains(query.search))&&
                                (!query.active.HasValue || (query.active.Value && a.Status == EntityStatusEnum.Online)) &&
                                (!query.passive.HasValue || (query.passive.Value && a.Status == EntityStatusEnum.Offline))
                ));
            var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
                .Take(res.PageSize).ToList();
            var mapData = _mapper.Map<List<PositionDto>>(resultData);
            res.SetData(mapData);
            res.TotalRecords = allData.Count();
            res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));

        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

    public async Task<ResultWithPagingDataDto<List<PositionDto>>> GetDeletedPositionListService(PositionQuery query)
    {
	    ResultWithPagingDataDto<List<PositionDto>> res = new ResultWithPagingDataDto<List<PositionDto>>(query.sayfa, query.search);
	    try
	    {
		    var allData = await Task.Run(() =>
			    _unitOfWork.ReadPositionRepository.GetAll(
				    orderBy: p => p.OrderByDescending(a => a.DeletedAt),
				    predicate: a => (a.Status == EntityStatusEnum.Archive) && 
				                    (string.IsNullOrEmpty(query.search) || a.Name.Contains(query.search))
			    ));
		    var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
			    .Take(res.PageSize).ToList();
		    var mapData = _mapper.Map<List<PositionDto>>(resultData);
		    res.SetData(mapData);
		    res.TotalRecords = allData.Count();
		    res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));

	    }
	    catch (Exception ex)
	    {
		    res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
	    }

	    return res;
    }

    public async Task<IResultWithDataDto<PositionDto>> GetUpdatePositionService(Guid id)
    {
        IResultWithDataDto<PositionDto> res = new ResultWithDataDto<PositionDto>();
        try
        {
            var resultData = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetByIdAsync(id));
            var mapData = _mapper.Map<PositionDto>(resultData.FirstOrDefault());
            res.SetData(mapData);
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }
        return res;
    }

    public async Task<List<PositionNameDto>> GetAllJustNames()
    {
        var entities = await Task.Run(() => _unitOfWork.ReadPositionRepository
	        .GetAll(predicate: p=> p.Status == EntityStatusEnum.Online,
		        orderBy: o=> o.OrderBy(p=> p.Name))
         .Select(p => new PositionNameDto { ID = p.ID, Name = p.Name })
        );
        return entities.ToList();
       
    }
}