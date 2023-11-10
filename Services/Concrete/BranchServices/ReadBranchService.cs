using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.BranchServices;

namespace Services.Concrete.BranchServices;

public class ReadBranchService : IReadBranchService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadBranchService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}
	public async Task<List<BranchDto>> GetAllAsync()
	{			
		var entities = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll());
		return _mapper.Map<List<BranchDto>>(entities.ToList());
	}

	public Task<BranchDto> GetSingleAsync()
	{
		throw new NotImplementedException(); 
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<IResultWithDataDto<List<BranchDto>>> GetAllOrderByAsync()
	{
		IResultWithDataDto<List<BranchDto>> res = new ResultWithDataDto<List<BranchDto>>();
		try
		{
			var resultData = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll(
				orderBy: p=> p.OrderBy(a=>a.Name),
				predicate: p=> (p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline)
				));
			var mapData = _mapper.Map<List<BranchDto>>(resultData.ToList());
			res.SetData(mapData);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
		
	}

	public async Task<ResultWithPagingDataDto<List<BranchDto>>> GetAllPagingOrderByAsync(int pageNumber,string search,bool passive)
	{
		ResultWithPagingDataDto<List<BranchDto>> res = new ResultWithPagingDataDto<List<BranchDto>>(pageNumber,search);
		try
		//String.IsNullOrEmpty(search) ? null : a => a.Name.Contains(search)
		{
                var allData = await Task.Run(() =>
                _unitOfWork.ReadBranchRepository.GetAll(
                    orderBy: p => p.OrderBy(a => a.Name),
                    predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
                                    (string.IsNullOrEmpty(search) || a.Name.Contains(search))&& 
                                    (passive == false || passive ==  (a.Status == EntityStatusEnum.Offline))
                    ));   
            var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<BranchDto>>(resultData);
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

	public async Task<ResultWithPagingDataDto<List<BranchDto>>> GetAllDeletedBranchPagingOrderByAsync(int pageNumber, string search)
	{
		ResultWithPagingDataDto<List<BranchDto>> res = new ResultWithPagingDataDto<List<BranchDto>>(pageNumber,search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadBranchRepository.GetAll(
					orderBy: p => p.OrderByDescending(a => a.DeletedAt),
					predicate: a => (a.Status == EntityStatusEnum.Archive ) && 
					                (string.IsNullOrEmpty(search) || a.Name.Contains(search))
				));   
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<BranchDto>>(resultData);
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

	public async Task<IResultWithDataDto<BranchDto>> GetByIdUpdate(int id)
    {
        IResultWithDataDto<BranchDto> res = new ResultWithDataDto<BranchDto>();
        try
        {
            var resultData = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetByIdAsync(id));
            var mapData = _mapper.Map<BranchDto>(resultData.FirstOrDefault());
            res.SetData(mapData);
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }
        return res;
    }
    public async Task<List<BranchNameDto>> GetAllJustNames()
    {
        var entities = await Task.Run(() => _unitOfWork.ReadBranchRepository
		.GetAll(predicate: p=> p.Status == EntityStatusEnum.Online,
        orderBy: o=> o.OrderBy(p=> p.Name))
		.Select(p => new BranchNameDto { ID = p.ID,Name = p.Name})
		);
        return entities.ToList();

    }
}