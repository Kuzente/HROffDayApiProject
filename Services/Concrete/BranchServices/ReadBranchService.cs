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
				predicate: p=> (p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline) &&
				               (string.IsNullOrWhiteSpace(query.search) || p.Name.Contains(query.search))&&
				               (query.isActive == null ? p.Status==EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline : (query.isActive == "active" ? p.Status == EntityStatusEnum.Online : p.Status == EntityStatusEnum.Offline)),
				orderBy: p => query.sortBy == "desc" ? p.OrderByDescending(a=>a.Name) : p.OrderBy(a=>a.Name)
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
		{
                var allData = await Task.Run(() =>
                _unitOfWork.ReadBranchRepository.GetAll(
                    predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
                                    (string.IsNullOrEmpty(query.search) || a.Name.Contains(query.search))&&
                                    (query.isActive == null ? a.Status==EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline : (query.isActive == "active" ? a.Status == EntityStatusEnum.Online : a.Status == EntityStatusEnum.Offline)),
                    orderBy: p => query.sortBy == "desc" ? p.OrderByDescending(a=>a.Name) : p.OrderBy(a=>a.Name)
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
					predicate: a => (a.Status == EntityStatusEnum.Archive ) && 
					                (string.IsNullOrEmpty(query.search) || a.Name.Contains(query.search)),
					orderBy: p =>
					{
						IOrderedQueryable<Branch> orderedBranch;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedBranch = query.sortName switch
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
							orderedBranch = p.OrderByDescending(a=> a.DeletedAt);
						}

						return orderedBranch;
					}
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