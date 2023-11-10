using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
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
	public async Task<List<PositionDto>> GetAllAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll());
		return _mapper.Map<List<PositionDto>>(entities.ToList());
	}

	public Task<PositionDto> GetSingleAsync()
	{
		throw new NotImplementedException();
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<bool> GetAnyByNameAsync(string name)
	{
		var result = await _unitOfWork.ReadPositionRepository.GetAny(p=> p.Name == name);
		return result;
	}

	public async Task<IResultWithDataDto<List<PositionDto>>> GetAllOrderByAsync()
	{
		IResultWithDataDto<List<PositionDto>> res = new ResultWithDataDto<List<PositionDto>>();
		try
		{
			var resultData = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll(
				orderBy: p=> p.OrderBy(a=>a.Name),
				predicate: p=> (p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline)
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

	public async Task<ResultWithPagingDataDto<List<PositionDto>>> GetAllPagingOrderByAsync(int pageNumber, string search, bool  passive)
    {
        ResultWithPagingDataDto<List<PositionDto>> res = new ResultWithPagingDataDto<List<PositionDto>>(pageNumber, search);
        try
        {
            var allData = await Task.Run(() =>
            _unitOfWork.ReadPositionRepository.GetAll(
                orderBy: p => p.OrderBy(a => a.Name),
                predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
                                (string.IsNullOrEmpty(search) || a.Name.Contains(search))&& 
                                (passive == false || passive ==  (a.Status == EntityStatusEnum.Offline))
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

    public async Task<ResultWithPagingDataDto<List<PositionDto>>> GetAllDeletedPositionPagingOrderByAsync(int pageNumber, string search)
    {
	    ResultWithPagingDataDto<List<PositionDto>> res = new ResultWithPagingDataDto<List<PositionDto>>(pageNumber, search);
	    try
	    {
		    var allData = await Task.Run(() =>
			    _unitOfWork.ReadPositionRepository.GetAll(
				    orderBy: p => p.OrderBy(a => a.Name),
				    predicate: a => (a.Status == EntityStatusEnum.Archive) && 
				                    (string.IsNullOrEmpty(search) || a.Name.Contains(search))
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

    public async Task<IResultWithDataDto<PositionDto>> GetByIdUpdate(int id)
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