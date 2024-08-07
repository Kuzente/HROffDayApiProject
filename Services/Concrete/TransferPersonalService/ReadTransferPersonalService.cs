using AutoMapper;
using Core.DTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Data.Abstract.TransferPersonalRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.TransferPersonalService;

namespace Services.Concrete.TransferPersonalService;

public class ReadTransferPersonalService : IReadTransferPersonalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReadTransferPersonalService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResultWithPagingDataDto<List<ReadTransferPersonalDto>>> GetTransferPersonalListByIdService(TransferPersonalQuery query)
    {
        ResultWithPagingDataDto<List<ReadTransferPersonalDto>> res = new ResultWithPagingDataDto<List<ReadTransferPersonalDto>>(query.sayfa, query.search);
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadTransferPersonalRepository.GetAll(
                    predicate: a =>
                        a.Personal_Id == query.id &&
                        (a.Status == EntityStatusEnum.Online) &&
                        (!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
                        (!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth ),
					include: a=> a.Include(p=>p.Personal),
                    orderBy: p => p.OrderByDescending(a => a.CreatedAt)
                ));
			var mappedData = _mapper.Map<List<ReadTransferPersonalDto>>(allData);
            var resultData = mappedData.Skip((res.PageNumber - 1) * res.PageSize)
                .Take(res.PageSize).ToList();
            res.SetData(mappedData);
            res.TotalRecords = allData.Count();
            res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
        }
        catch (Exception e)
        {
            res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

    public async Task<IResultWithDataDto<List<ReadTransferPersonalDto>>> ExcelGetTransferPersonalListByIdService(TransferPersonalQuery query)
    {
        IResultWithDataDto<List<ReadTransferPersonalDto>> res = new ResultWithDataDto<List<ReadTransferPersonalDto>>();
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadTransferPersonalRepository.GetAll(
                    predicate: a =>
                        a.Personal_Id == query.id &&
                        (a.Status == EntityStatusEnum.Online) &&
                        (!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
                        (!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth ),
					include: a => a.Include(p => p.Personal),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
                ));
            var mappedData = _mapper.Map<List<ReadTransferPersonalDto>>(allData);
            res.SetData(mappedData);
        }
        catch (Exception e)
        {
            res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

	public async Task<ResultWithPagingDataDto<List<ReadTransferPersonalDto>>> GetTransferPersonalListService(TransferPersonalQuery query)
	{
		ResultWithPagingDataDto<List<ReadTransferPersonalDto>> res = new ResultWithPagingDataDto<List<ReadTransferPersonalDto>>(query.sayfa, query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadTransferPersonalRepository.GetAll(
					predicate: a =>
                        (a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
						(a.Status == EntityStatusEnum.Online) &&
						(!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth) &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
					include: a => a.Include(p => p.Personal),
					orderBy: p =>
					{
						IOrderedQueryable<TransferPersonal> orderedTransfers;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedTransfers = query.sortName switch
							{
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt),
								"oldBranch" => query.sortBy == "asc"
									? p.OrderBy(a => a.OldBranch).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.OldBranch).ThenByDescending(a => a.CreatedAt),
								"newBranch" => query.sortBy == "asc"
								? p.OrderBy(a => a.NewBranch).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.NewBranch).ThenByDescending(a => a.CreatedAt),
								"oldPosition" => query.sortBy == "asc"
									? p.OrderBy(a => a.OldPosition).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.OldPosition).ThenByDescending(a => a.CreatedAt),
								"newPosition" => query.sortBy == "asc"
								? p.OrderBy(a => a.NewPosition).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.NewPosition).ThenByDescending(a => a.CreatedAt),
								"createdAt" => query.sortBy == "asc"
								? p.OrderBy(a => a.CreatedAt)
								: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a => a.CreatedAt)
							};
						}
						else
						{
							orderedTransfers = p.OrderByDescending(a => a.CreatedAt);
						}

						return orderedTransfers;
					}
				));
			var mappedData = _mapper.Map<List<ReadTransferPersonalDto>>(allData);
			var resultData = mappedData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			res.SetData(resultData);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}
	public async Task<IResultWithDataDto<List<ReadTransferPersonalDto>>> ExcelGetTransferPersonalListService(TransferPersonalQuery query)
	{
		IResultWithDataDto<List<ReadTransferPersonalDto>> res = new ResultWithDataDto<List<ReadTransferPersonalDto>>();
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadTransferPersonalRepository.GetAll(
					predicate: a =>
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
						(a.Status == EntityStatusEnum.Online) &&
						(!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
					include: a => a.Include(p => p.Personal),
					orderBy: p =>
					{
						IOrderedQueryable<TransferPersonal> orderedTransfers;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedTransfers = query.sortName switch
							{
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt),
								"oldBranch" => query.sortBy == "asc"
									? p.OrderBy(a => a.OldBranch).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.OldBranch).ThenByDescending(a => a.CreatedAt),
								"newBranch" => query.sortBy == "asc"
								? p.OrderBy(a => a.NewBranch).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.NewBranch).ThenByDescending(a => a.CreatedAt),
								"oldPosition" => query.sortBy == "asc"
									? p.OrderBy(a => a.OldPosition).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.OldPosition).ThenByDescending(a => a.CreatedAt),
								"newPosition" => query.sortBy == "asc"
								? p.OrderBy(a => a.NewPosition).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.NewPosition).ThenByDescending(a => a.CreatedAt),
								"createdAt" => query.sortBy == "asc"
								? p.OrderBy(a => a.CreatedAt)
								: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a => a.CreatedAt)
							};
						}
						else
						{
							orderedTransfers = p.OrderByDescending(a => a.CreatedAt);
						}

						return orderedTransfers;
					}
				));
			var mappedData = _mapper.Map<List<ReadTransferPersonalDto>>(allData);
			res.SetData(mappedData);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}
}