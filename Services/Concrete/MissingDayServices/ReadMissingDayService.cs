using Core.DTOs;
using Core.DTOs.MissingDayDtos.ReadDtos;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Data.Abstract.MissingDayRepositories;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.MissingDayServices;

namespace Services.Concrete.MissingDayServices;

public class ReadMissingDayService : IReadMissingDayService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadMissingDayService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
                        (!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
                        (!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth ),
                    //orderBy: p => p.OrderByDescending(a => a.CreatedAt)
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
                        (!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
                        (!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth ),
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
}