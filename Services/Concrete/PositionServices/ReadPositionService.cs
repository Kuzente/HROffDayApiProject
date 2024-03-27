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
				predicate: p=> (p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline) &&
				               (string.IsNullOrEmpty(query.search) || p.Name.ToLower().Contains(query.search.ToLower()))&&
				               (query.isActive == null ? p.Status==EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline : (query.isActive == "active" ? p.Status == EntityStatusEnum.Online : p.Status == EntityStatusEnum.Offline)),
				orderBy: p => query.sortBy == "desc" ? p.OrderByDescending(a=>a.Name) : p.OrderBy(a=>a.Name)
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
                predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
                                (string.IsNullOrEmpty(query.search) || a.Name.ToLower().Contains(query.search.ToLower()))&&
                                (query.isActive == null ? a.Status==EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline : (query.isActive == "active" ? a.Status == EntityStatusEnum.Online : a.Status == EntityStatusEnum.Offline)),
                orderBy: p => query.sortBy == "desc" ? p.OrderByDescending(a=>a.Name) : p.OrderBy(a=>a.Name)
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
				    predicate: a => (a.Status == EntityStatusEnum.Archive) && 
				                    (string.IsNullOrEmpty(query.search) || a.Name.ToLower().Contains(query.search.ToLower())),
				    orderBy: p =>
				    {
					    IOrderedQueryable<Position> orderedPosition;
					    if (query.sortName is not null && query.sortBy is not null)
					    {
						    orderedPosition = query.sortName switch
						    {
							    "nameSurname" => query.sortBy == "asc"
								    ? p.OrderBy(a => a.Name)
								    : p.OrderByDescending(a => a.Name),
							    "deletedAt" => query.sortBy == "asc"
								    ? p.OrderBy(a => a.DeletedAt)
								    : p.OrderByDescending(a => a.DeletedAt),
							    _ => p.OrderByDescending(a=> a.DeletedAt)
						    };
					    }
					    else
					    {
						    orderedPosition = p.OrderByDescending(a=> a.DeletedAt);
					    }

					    return orderedPosition;
				    }
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