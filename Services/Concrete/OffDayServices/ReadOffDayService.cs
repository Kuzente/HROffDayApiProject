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
						a.Personal.Status == EntityStatusEnum.Online &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search)),
					include: p=>p.Include(a=>a.Personal).Include(a=>a.Personal.Branch).Include(a=> a.Personal.Position),
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

	public async Task<ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>> GetSecondWaitingOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.WaitingForSecond) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.branchName) || a.Personal.Branch.Name.Contains(query.branchName))&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search)),
					include: p=>p.Include(a=>a.Personal).Include(a=>a.Personal.Branch).Include(a=> a.Personal.Position),
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

	public async Task<ResultWithPagingDataDto<List<ReadRejectedOffDayListDto>>> GetRejectedOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<List<ReadRejectedOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadRejectedOffDayListDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.Rejected) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search)),
					include: p=>p.Include(a=>a.Personal).Include(a=> a.Personal.Branch).Include(a=>a.Personal.Position),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadRejectedOffDayListDto>>(resultData);
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

	public async Task<ResultWithPagingDataDto<List<ReadApprovedOffDayListDto>>> GetApprovedOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<List<ReadApprovedOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadApprovedOffDayListDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.Approved ) &&
						(a.Personal.Status == EntityStatusEnum.Online ||
						 a.Personal.Status == EntityStatusEnum.Offline) &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear || a.EndDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth || a.EndDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search))&&
						(string.IsNullOrEmpty(query.branchName) || a.Personal.Branch.Name.Contains(query.branchName))&&
						(string.IsNullOrEmpty(query.positionName) || a.Personal.Position.Name.Contains(query.positionName)),
					include: p=>p.Include(a=>a.Personal)
												.ThenInclude(a=>a.Branch)
												.Include(a=>a.Personal.Position),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadApprovedOffDayListDto>>(resultData);
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

	public async Task<ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>>> GetDeletedOffDaysListService(int sayfa , string search)
	{
		ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>>(sayfa,search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Deleted ) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(string.IsNullOrEmpty(search) || a.Personal.NameSurname.Contains(search)),
					include: p=>p.Include(a=>a.Personal)
						.ThenInclude(a=>a.Branch)
						.Include(a=>a.Personal.Position),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadDeletedOffDayListDto>>(resultData);
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

	public async Task<ResultWithPagingDataDto<List<ReadPersonalOffDayListDto>>> GetPersonalOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<List<ReadPersonalOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadPersonalOffDayListDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.Approved) &&
						a.Personal.Status == EntityStatusEnum.Online && a.Personal_Id == query.id &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear || a.EndDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth || a.EndDate.Month == query.filterMonth),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadPersonalOffDayListDto>>(resultData);
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
			var resultData = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => p.ID == id && 
                p.Personal.Status == EntityStatusEnum.Online,
				include: p=> p.Include(a=>a.Personal)
											.ThenInclude(a=>a.Branch)
											.Include(a=>a.Personal)
											.ThenInclude(a=>a.Position));
			if(resultData is null)
				return res.SetStatus(false).SetErr("OffDay Not Found").SetMessage("İlgili Personele Ait İzin Bulunamadı!!!");
			var mapData = _mapper.Map<ReadWaitingOffDayEditDto>(resultData);
			res.SetData(mapData);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultWithDataDto<List<ReadApprovedOffDayListDto>>> GetExcelApprovedOffDayListService(OffdayQuery query)
	{
		IResultWithDataDto<List<ReadApprovedOffDayListDto>> res = new ResultWithDataDto<List<ReadApprovedOffDayListDto>>();
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.Approved ) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(!query.id.HasValue || a.Personal_Id == query.id) &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear || a.EndDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth || a.EndDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search))&&
						(string.IsNullOrEmpty(query.branchName) || a.Personal.Branch.Name.Contains(query.branchName))&&
						(string.IsNullOrEmpty(query.positionName) || a.Personal.Position.Name.Contains(query.positionName)),
					include: p=>p.Include(a=>a.Personal).ThenInclude(a=>a.Branch).Include(a=>a.Personal.Position),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var mapData = _mapper.Map<List<ReadApprovedOffDayListDto>>(allData);
			res.SetData(mapData);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultWithDataDto<ReadApprovedOffDayFormExcelExportDto>> GetApprovedOffDayExcelFormService(Guid id)
	{
		IResultWithDataDto<ReadApprovedOffDayFormExcelExportDto> res = new ResultWithDataDto<ReadApprovedOffDayFormExcelExportDto>();
		try
		{
			var offday = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
					predicate:p=> p.ID == id && p.OffDayStatus == OffDayStatusEnum.Approved,
					include: p=> p.Include(a=> a.Personal).Include(a=>a.Personal.Branch).Include(a=>a.Personal.Position)
			);
			if (offday is null)
				return res.SetStatus(false).SetErr("Offday is not find").SetMessage("İlgili izin bulunamadı!");
			var mapData = _mapper.Map<ReadApprovedOffDayFormExcelExportDto>(offday);
			res.SetData(mapData);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}
}