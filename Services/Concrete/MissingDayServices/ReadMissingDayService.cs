using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.MissingDayDtos.ReadDtos;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Data.Abstract.MissingDayRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.MissingDayServices;

namespace Services.Concrete.MissingDayServices;

public class ReadMissingDayService : IReadMissingDayService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReadMissingDayService(IUnitOfWork unitOfWork, IMapper mapper)
    {
	    _unitOfWork = unitOfWork;
	    _mapper = mapper;
    }

    public async Task<ResultWithPagingDataDto<List<ReadMissingDayDto>>> GetMissingDayListByIdService(MissingDayQuery query)
    {
        ResultWithPagingDataDto<List<ReadMissingDayDto>> res = new ResultWithPagingDataDto<List<ReadMissingDayDto>>(query.sayfa, query.search);
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadMissingDayRepository.GetAll(
                    predicate: a =>
                        a.Personal_Id == query.id &&
                        (a.Status == EntityStatusEnum.Online) &&
						(!query.filterYear.HasValue ||
						a.EndOffDayDate.Year == query.filterYear ||
						a.StartOffdayDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue ||
						a.EndOffDayDate.Month == query.filterMonth ||
						a.StartOffdayDate.Month == query.filterMonth
						),
                    orderBy: p =>
					{
						IOrderedQueryable<MissingDay> orderedquery;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedquery = query.sortName switch
							{
								"startOffDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartOffdayDate)
									: p.OrderByDescending(a => a.StartOffdayDate),
								"EndOffDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.EndOffDayDate)
									: p.OrderByDescending(a => a.EndOffDayDate),
								"startJobDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartJobDate)
									: p.OrderByDescending(a => a.StartJobDate),
								"reason" => query.sortBy == "asc"
									? p.OrderBy(a => a.Reason)
									: p.OrderByDescending(a => a.Reason),
								"createdAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.CreatedAt)
									: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a=> a.CreatedAt)
							};
						}
						else
						{
							orderedquery = p.OrderByDescending(a=> a.CreatedAt);
						}

						return orderedquery;
					}
                )
                );
            var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
            var allDataMapped = allData.Select(a => new ReadMissingDayDto
            {
                ID = a.ID,
                NameSurname = a.Personal.NameSurname,
                IdentificationNumber = a.Personal.IdentificationNumber,
                Branch_Id = a.Branch_Id,
                StartOffdayDate = a.StartOffdayDate,
                EndOffDayDate = a.EndOffDayDate,
                StartJobDate = a.StartJobDate,
                Reason = a.Reason,
                CreatedAt = a.CreatedAt
            });
            if(allDataMapped is null || branchList.IsNullOrEmpty()) res.SetStatus(false).SetErr("Branch or MissingDay is not found").SetMessage("Şube veya Eksik Gün Bulunamadı...");
            var resultData = allDataMapped.Skip((res.PageNumber - 1) * res.PageSize)
                .Take(res.PageSize).ToList();
            foreach (var item in resultData)
            {
                var branch = branchList.FirstOrDefault(b => b.ID == item.Branch_Id);
                if (branch is not null)
                {
                    item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
                }
            }
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

    public async Task<IResultWithDataDto<List<ReadMissingDayDto>>> ExcelGetPersonalMissingDayListByIdService(MissingDayQuery query)
    {
	    IResultWithDataDto<List<ReadMissingDayDto>> res = new ResultWithDataDto<List<ReadMissingDayDto>>();
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadMissingDayRepository.GetAll(
                    predicate: a =>
                        a.Personal_Id == query.id &&
                        (a.Status == EntityStatusEnum.Online) &&
						(!query.filterYear.HasValue ||
						a.EndOffDayDate.Year == query.filterYear ||
						a.StartOffdayDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue ||
						a.EndOffDayDate.Month == query.filterMonth ||
						a.StartOffdayDate.Month == query.filterMonth
						),
                    orderBy: p =>
					{
						IOrderedQueryable<MissingDay> orderedquery;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedquery = query.sortName switch
							{
								"startOffDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartOffdayDate)
									: p.OrderByDescending(a => a.StartOffdayDate),
								"EndOffDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.EndOffDayDate)
									: p.OrderByDescending(a => a.EndOffDayDate),
								"startJobDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartJobDate)
									: p.OrderByDescending(a => a.StartJobDate),
								"reason" => query.sortBy == "asc"
									? p.OrderBy(a => a.Reason)
									: p.OrderByDescending(a => a.Reason),
								"createdAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.CreatedAt)
									: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a=> a.CreatedAt)
							};
						}
						else
						{
							orderedquery = p.OrderByDescending(a=> a.CreatedAt);
						}

						return orderedquery;
					}
                )
                );
            var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
            var allDataMapped = allData.Select(a => new ReadMissingDayDto
            {
                ID = a.ID,
                NameSurname = a.Personal.NameSurname,
                IdentificationNumber = a.Personal.IdentificationNumber,
                Branch_Id = a.Branch_Id,
                StartOffdayDate = a.StartOffdayDate,
                EndOffDayDate = a.EndOffDayDate,
                StartJobDate = a.StartJobDate,
                Reason = a.Reason,
                CreatedAt = a.CreatedAt
            }).ToList();
            if(allDataMapped is null || branchList.IsNullOrEmpty()) res.SetStatus(false).SetErr("Branch or MissingDay is not found").SetMessage("Şube veya Eksik Gün Bulunamadı...");
           
            foreach (var item in allDataMapped)
            {
                var branch = branchList.FirstOrDefault(b => b.ID == item.Branch_Id);
                if (branch is not null)
                {
                    item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
                }
            }
            res.SetData(allDataMapped);
        }
        catch (Exception e)
        {
            res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

	public async Task<ResultWithPagingDataDto<List<ReadMissingDayDto>>> GetMissingDayListService(MissingDayQuery query)
	{
		ResultWithPagingDataDto<List<ReadMissingDayDto>> res = new ResultWithPagingDataDto<List<ReadMissingDayDto>>(query.sayfa, query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadMissingDayRepository.GetAll(
					predicate: a =>
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
						(a.Status == EntityStatusEnum.Online) &&
						a.Branch.Status == EntityStatusEnum.Online &&
						(!query.filterYear.HasValue ||
						a.EndOffDayDate.Year == query.filterYear ||
						a.StartOffdayDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue ||
						a.EndOffDayDate.Month == query.filterMonth ||
						a.StartOffdayDate.Month == query.filterMonth
						)&&
						(string.IsNullOrEmpty(query.filterReason) || a.Reason.Contains(query.filterReason)) &&
						(string.IsNullOrEmpty(query.filterBranch) || a.Branch_Id.ToString().Contains(query.filterBranch)) &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
					include: p=> p.Include(a=> a.Personal).Include(a=> a.Branch),
					orderBy: p =>
					{
						IOrderedQueryable<MissingDay> orderedquery;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedquery = query.sortName switch
							{
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt),
								"identificationNumber" => query.sortBy == "asc"
								? p.OrderBy(a => a.Personal.IdentificationNumber).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.Personal.IdentificationNumber).ThenByDescending(a => a.CreatedAt),
								"branchName" => query.sortBy == "asc"
								? p.OrderBy(a => a.Branch.Name).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.Branch.Name).ThenByDescending(a => a.CreatedAt),
								"startOffDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartOffdayDate).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.StartOffdayDate).ThenByDescending(a => a.CreatedAt),
								"EndOffDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.EndOffDayDate).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.EndOffDayDate).ThenByDescending(a => a.CreatedAt),
								"startJobDayDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartJobDate).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.StartJobDate).ThenByDescending(a => a.CreatedAt),
								"reason" => query.sortBy == "asc"
									? p.OrderBy(a => a.Reason).ThenByDescending(a => a.CreatedAt)
									: p.OrderByDescending(a => a.Reason).ThenByDescending(a => a.CreatedAt),
								"createdAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.CreatedAt)
									: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a => a.CreatedAt)
							};
						}
						else
						{
							orderedquery = p.OrderByDescending(a => a.CreatedAt);
						}

						return orderedquery;
					}
				)
				);
			
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mappedData = _mapper.Map<List<ReadMissingDayDto>>(resultData);
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

	public async Task<IResultWithDataDto<List<ReadMissingDayDto>>> ExcelGetPersonalMissingDayListService(MissingDayQuery query)
	{
		IResultWithDataDto<List<ReadMissingDayDto>> res = new ResultWithDataDto<List<ReadMissingDayDto>>();
		try
		{
			var allData = await Task.Run(() =>
							_unitOfWork.ReadMissingDayRepository.GetAll(
								predicate: a =>
									(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
									(a.Status == EntityStatusEnum.Online) &&
									a.Branch.Status == EntityStatusEnum.Online &&
									(!query.filterYear.HasValue ||
									a.EndOffDayDate.Year == query.filterYear ||
									a.StartOffdayDate.Year == query.filterYear) &&
									(!query.filterMonth.HasValue ||
									a.EndOffDayDate.Month == query.filterMonth ||
									a.StartOffdayDate.Month == query.filterMonth
									) &&
									(string.IsNullOrEmpty(query.filterReason) || a.Reason.Contains(query.filterReason)) &&
									(string.IsNullOrEmpty(query.filterBranch) || a.Branch_Id.ToString().Contains(query.filterBranch)) &&
									(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
								include: p => p.Include(a => a.Personal).Include(a => a.Branch),
								orderBy: p =>
								{
									IOrderedQueryable<MissingDay> orderedquery;
									if (query.sortName is not null && query.sortBy is not null)
									{
										orderedquery = query.sortName switch
										{
											"nameSurname" => query.sortBy == "asc"
												? p.OrderBy(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt)
												: p.OrderByDescending(a => a.Personal.NameSurname).ThenByDescending(a => a.CreatedAt),
											"identificationNumber" => query.sortBy == "asc"
											? p.OrderBy(a => a.Personal.IdentificationNumber).ThenByDescending(a => a.CreatedAt)
											: p.OrderByDescending(a => a.Personal.IdentificationNumber).ThenByDescending(a => a.CreatedAt),
											"branchName" => query.sortBy == "asc"
											? p.OrderBy(a => a.Branch.Name).ThenByDescending(a => a.CreatedAt)
											: p.OrderByDescending(a => a.Branch.Name).ThenByDescending(a => a.CreatedAt),
											"startOffDayDate" => query.sortBy == "asc"
												? p.OrderBy(a => a.StartOffdayDate).ThenByDescending(a => a.CreatedAt)
												: p.OrderByDescending(a => a.StartOffdayDate).ThenByDescending(a => a.CreatedAt),
											"EndOffDayDate" => query.sortBy == "asc"
												? p.OrderBy(a => a.EndOffDayDate).ThenByDescending(a => a.CreatedAt)
												: p.OrderByDescending(a => a.EndOffDayDate).ThenByDescending(a => a.CreatedAt),
											"startJobDayDate" => query.sortBy == "asc"
												? p.OrderBy(a => a.StartJobDate).ThenByDescending(a => a.CreatedAt)
												: p.OrderByDescending(a => a.StartJobDate).ThenByDescending(a => a.CreatedAt),
											"reason" => query.sortBy == "asc"
												? p.OrderBy(a => a.Reason).ThenByDescending(a => a.CreatedAt)
												: p.OrderByDescending(a => a.Reason).ThenByDescending(a => a.CreatedAt),
											"createdAt" => query.sortBy == "asc"
												? p.OrderBy(a => a.CreatedAt)
												: p.OrderByDescending(a => a.CreatedAt),
											_ => p.OrderByDescending(a => a.CreatedAt)
										};
									}
									else
									{
										orderedquery = p.OrderByDescending(a => a.CreatedAt);
									}

									return orderedquery;
								}
							)
							);
			var mappedData = _mapper.Map<List<ReadMissingDayDto>>(allData);
			res.SetData(mappedData);

		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}
}