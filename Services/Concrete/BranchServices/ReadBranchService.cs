using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.BranchDTOs.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
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

	public async Task<IResultWithDataDto<List<BranchDto>>> GetExcelBranchListService(BranchQuery query)
	{
		IResultWithDataDto<List<BranchDto>> res = new ResultWithDataDto<List<BranchDto>>();
		try
		{
			var resultData = await Task.Run(() => 
				_unitOfWork.ReadBranchRepository.GetAll(
				orderBy: p=> p.OrderBy(a=>a.Name),
				predicate: p=> (p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline) &&
				               (string.IsNullOrWhiteSpace(query.search) || p.Name.Contains(query.search))&&
                               (!query.active.HasValue || (query.active.Value && p.Status == EntityStatusEnum.Online)) &&
                               (!query.passive.HasValue || (query.passive.Value && p.Status == EntityStatusEnum.Offline))
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

	public async Task<ResultWithPagingDataDto<List<BranchDto>>> GetBranchListService(BranchQuery query)
	{
		ResultWithPagingDataDto<List<BranchDto>> res = new ResultWithPagingDataDto<List<BranchDto>>(query.sayfa,query.search);
		try
		//String.IsNullOrEmpty(search) ? null : a => a.Name.Contains(search)
		{
                var allData = await Task.Run(() =>
                _unitOfWork.ReadBranchRepository.GetAll(
                    orderBy: p => p.OrderBy(a => a.Name),
                    predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
                                    (string.IsNullOrEmpty(query.search) || a.Name.Contains(query.search))&&
                                    (!query.active.HasValue || (query.active.Value && a.Status == EntityStatusEnum.Online)) &&
									(!query.passive.HasValue || (query.passive.Value && a.Status == EntityStatusEnum.Offline))
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

	public async Task<ResultWithPagingDataDto<List<BranchDto>>> GetDeletedBranchListService(BranchQuery query)
	{
		ResultWithPagingDataDto<List<BranchDto>> res = new ResultWithPagingDataDto<List<BranchDto>>(query.sayfa, query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadBranchRepository.GetAll(
					orderBy: p => p.OrderByDescending(a => a.DeletedAt),
					predicate: a => (a.Status == EntityStatusEnum.Archive ) && 
					                (string.IsNullOrEmpty(query.search) || a.Name.Contains(query.search))
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

	public async Task<IResultWithDataDto<BranchDto>> GetUpdateBranchService(Guid id)
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