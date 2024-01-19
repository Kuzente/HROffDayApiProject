using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.PassivePersonalDtos;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.OffDayServices;

namespace Services.Concrete.OffDayServices;

public class ReadOffDayService : IReadOffDayService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadOffDayService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}


	public async Task<ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>> GetFirstWaitingOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.WaitingForFirst) &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search)),
					include: p=>p.Include(a=>a.Personal),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadWaitingOffDayListDto>>(resultData);
			res.SetData(mapData);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultWithDataDto<ReadWaitingOffDayEditDto>> GetFirstWaitingOffDayByIdService(Guid id)
	{
		IResultWithDataDto<ReadWaitingOffDayEditDto> res = new ResultWithDataDto<ReadWaitingOffDayEditDto>();
		try
		{
			var resultData = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(predicate: p => p.ID == id,
				include: p=> p.Include(a=>a.Personal)
											.ThenInclude(a=>a.Branch)
											.Include(a=>a.Personal)
											.ThenInclude(a=>a.Position));
			var mapData = _mapper.Map<ReadWaitingOffDayEditDto>(resultData);
			res.SetData(mapData);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}
}