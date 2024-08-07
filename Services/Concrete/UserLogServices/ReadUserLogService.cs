using AutoMapper;
using Core.DTOs;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.UserLogDTOs.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.UserLogServices;

namespace Services.Concrete.UserLogServices;

public class ReadUserLogService : IReadUserLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReadUserLogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResultWithDataDto<List<HeaderLastFiveLogDto>>> GetLastFiveLogService()
    {
        IResultWithDataDto<List<HeaderLastFiveLogDto>> result = new ResultWithDataDto<List<HeaderLastFiveLogDto>>();
        try
        {
            var logs = await Task.Run(() => _unitOfWork.ReadUserLogRepository.GetAll(
                predicate: p => p.Status == EntityStatusEnum.Online,
                include: p=> p.Include(a=> a.User),
                orderBy: p => p.OrderByDescending(a => a.CreatedAt)
            ).Take(5));
            var mappedData = _mapper.Map<List<HeaderLastFiveLogDto>>(logs);
            result.SetData(mappedData);
        }
        catch (Exception ex)
        {
            result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }

	public async Task<ResultWithPagingDataDto<List<UsersLogListDto>>> GetUsersLogsListService(UserLogQuery query)
	{
		ResultWithPagingDataDto<List<UsersLogListDto>> res = new ResultWithPagingDataDto<List<UsersLogListDto>>(query.sayfa, query.search);
        try
        {
            var logs = await Task.Run(() => _unitOfWork.ReadUserLogRepository.GetAll(
                predicate: p=> p.Status == EntityStatusEnum.Online &&
				(!query.filterYear.HasValue || p.CreatedAt.Year == query.filterYear) &&
				(!query.filterMonth.HasValue || p.CreatedAt.Month == query.filterMonth) &&
				(string.IsNullOrEmpty(query.search) || p.User.Username.ToLower().Contains(query.search.ToLower())) &&
				(!query.LogType.HasValue || p.LogType == query.LogType),
				include: p => p.Include(a => a.User),
				orderBy: p =>
				{
					IOrderedQueryable<UserLog> orderedUserLogs;
					if (query.sortName is not null && query.sortBy is not null)
					{
						orderedUserLogs = query.sortName switch
						{
							"username" => query.sortBy == "asc"
								? p.OrderBy(a => a.User.Username).ThenByDescending(a=>a.CreatedAt)
								: p.OrderByDescending(a => a.User.Username).ThenByDescending(a => a.CreatedAt),
							"logType" => query.sortBy == "asc"
								? p.OrderBy(a => a.LogType).ThenByDescending(a => a.CreatedAt)
								: p.OrderByDescending(a => a.LogType).ThenByDescending(a => a.CreatedAt),
							"createdAt" => query.sortBy == "asc"
							? p.OrderBy(a => a.CreatedAt)
							: p.OrderByDescending(a => a.CreatedAt),
							_ => p.OrderByDescending(a => a.CreatedAt)
						};
					}
					else
					{
						orderedUserLogs = p.OrderByDescending(a => a.CreatedAt);
					}

					return orderedUserLogs;
				}

				));
			var resultData = logs.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<UsersLogListDto>>(resultData);
			res.SetData(mapData);
			res.TotalRecords = logs.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
		}
        catch (Exception ex)
        {

			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
        return res;
	}
}