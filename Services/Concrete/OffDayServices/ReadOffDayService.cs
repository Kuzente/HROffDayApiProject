using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.PassivePersonalDtos;
using Core.Entities;
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
					include: p=>p.Include(a=>a.Personal),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			if(allData is null||branchList is null || positionList is null)
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("İzinler Bulunamadı...");
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadWaitingOffDayListDto>>(resultData);
			foreach (var item in mapData)
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == item.PositionId);
				if (branch is not null && position is not null)
				{
					item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					item.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
			}
			res.SetData(mapData);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling(res.TotalRecords / (double)res.PageSize));
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
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search)),
					include: p=>p.Include(a=>a.Personal),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			if(allData is null||branchList is null || positionList is null)
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("İzinler Bulunamadı...");
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadWaitingOffDayListDto>>(resultData);
			foreach (var item in mapData)
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == item.PositionId);
				if (branch is not null && position is not null)
				{
					item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					item.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
			}
			res.SetData(mapData);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling(res.TotalRecords / (double)res.PageSize));
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
					include: p=>p.Include(a=>a.Personal),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			if(allData is null||branchList is null || positionList is null)
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("İzinler Bulunamadı...");
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadRejectedOffDayListDto>>(resultData);
			foreach (var item in mapData)
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == item.PositionId);
				if (branch is not null && position is not null)
				{
					item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					item.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
			}
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

	public async Task<ResultWithPagingDataDto<ReadApprovedOffDayDto>> GetApprovedOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<ReadApprovedOffDayDto> res = new ResultWithPagingDataDto<ReadApprovedOffDayDto>(query.sayfa,query.search);
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
							(string.IsNullOrEmpty(query.isFreedayLeave) || a.LeaveByFreeDay > 0),
					
					include: p=>p.Include(a=>a.Personal),
					orderBy: p =>
					{
						IOrderedQueryable<OffDay> orderedOffdays;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedOffdays = query.sortName switch
							{
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname)
									: p.OrderByDescending(a => a.Personal.NameSurname),
								"countLeave" => query.sortBy == "asc"
									? p.OrderBy(a => a.CountLeave)
									: p.OrderByDescending(a => a.CountLeave),
								"createdAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.CreatedAt)
									: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a=> a.CreatedAt)
							};
						}
						else
						{
							orderedOffdays = p.OrderByDescending(a=> a.CreatedAt);
						}

						return orderedOffdays;
					}
				));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			ReadApprovedOffDayDto mapDataDto = new()
			{
				ReadApprovedOffDayListDtos = new(),
				ReadApprovedOffDayGetBranchesList = new(),
				ReadApprovedOffDayGetPositionsList = new()
			};
			if(allData is null||branchList is null || positionList is null)
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("İzinler Bulunamadı...");
			foreach (var item in allData.Select(a=>a.BranchId).Distinct().ToList())
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item);
				if (branch is not null)
				{
					mapDataDto.ReadApprovedOffDayGetBranchesList.Add(new ReadApprovedOffDayGetBranches
					{
						BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name,
						BranchNameValue = branch.Name,
					});
				}
			}
			foreach (var item in allData.Select(a=>a.PositionId).Distinct().ToList())
			{
				var position = positionList.FirstOrDefault(p => p.ID == item);
				if (position is not null)
				{
					mapDataDto.ReadApprovedOffDayGetPositionsList.Add(new ReadApprovedOffDayGetPositions()
					{
						PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name,
						PositionNameValue = position.Name
					});
				}
			}
			if (!string.IsNullOrEmpty(query.branchName))
			{
				allData = allData.Where(a => branchList.Any(b => b.ID == a.BranchId && b.Name.Contains(query.branchName)));
			}
			if (!string.IsNullOrEmpty(query.positionName))
			{
				allData = allData.Where(a => positionList.Any(b => b.ID == a.PositionId && b.Name.Contains(query.positionName)));
			}
			if (query.sortBy is not null && query.sortName is "branchName" or "positionName")
			{
				allData = query.sortName switch
				{
					"branchName" => query.sortBy == "asc"
						? allData.OrderBy(a => branchList.FirstOrDefault(b => b.ID == a.BranchId).Name)
						: allData.OrderByDescending(a => branchList.FirstOrDefault(b => b.ID == a.BranchId).Name),
					"positionName" => query.sortBy == "asc"
						? allData.OrderBy(a => positionList.FirstOrDefault(p => p.ID == a.PositionId).Name)
						: allData.OrderByDescending(a => positionList.FirstOrDefault(p => p.ID == a.PositionId).Name),
					_ => allData.OrderByDescending(a=> a.DeletedAt)
				};
			}
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			mapDataDto.ReadApprovedOffDayListDtos = _mapper.Map<List<ReadApprovedOffDayListDto>>(resultData);
			foreach (var item in mapDataDto.ReadApprovedOffDayListDtos)
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == item.PositionId);
				if (branch is not null && position is not null)
				{
					item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					item.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
			}
			
			res.SetData(mapDataDto);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>>> GetDeletedOffDaysListService(OffdayQuery query)
	{
		ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>> res = new ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Deleted ) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search)),
					include: p=>p.Include(a=>a.Personal),
					orderBy: p =>
					{
						IOrderedQueryable<OffDay> orderedOffdays;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedOffdays = query.sortName switch
							{
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname)
									: p.OrderByDescending(a => a.Personal.NameSurname),
								"startDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.StartDate)
									: p.OrderByDescending(a => a.StartDate),
								"endDate" => query.sortBy == "asc"
									? p.OrderBy(a => a.EndDate)
									: p.OrderByDescending(a => a.EndDate),
								"countLeave" => query.sortBy == "asc"
									? p.OrderBy(a => a.CountLeave)
									: p.OrderByDescending(a => a.CountLeave),
								"deletedAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.DeletedAt)
									: p.OrderByDescending(a => a.DeletedAt),
								_ => p.OrderByDescending(a=> a.DeletedAt)
							};
						}
						else
						{
							orderedOffdays = p.OrderByDescending(a=> a.DeletedAt);
						}

						return orderedOffdays;
					}
				));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			if (query.sortBy is not null && query.sortName is "branchName" or "positionName")
			{
				allData = query.sortName switch
				{
					"branchName" => query.sortBy == "asc"
						? allData.OrderBy(a => branchList.FirstOrDefault(b => b.ID == a.BranchId).Name)
						: allData.OrderByDescending(a => branchList.FirstOrDefault(b => b.ID == a.BranchId).Name),
					"positionName" => query.sortBy == "asc"
						? allData.OrderBy(a => positionList.FirstOrDefault(p => p.ID == a.PositionId).Name)
						: allData.OrderByDescending(a => positionList.FirstOrDefault(p => p.ID == a.PositionId).Name),
					_ => allData.OrderByDescending(a=> a.DeletedAt)
				};
			}
			
			if(allData is null||branchList is null || positionList is null)
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("İzinler Bulunamadı...");
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadDeletedOffDayListDto>>(resultData);
			foreach (var item in mapData)
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == item.PositionId);
				if (branch is not null && position is not null)
				{
					item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					item.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
			}
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

	public async Task<IResultWithDataDto<ReadWaitingOffDayEditDto>> GetOffDayByIdService(Guid id)
	{
		IResultWithDataDto<ReadWaitingOffDayEditDto> res = new ResultWithDataDto<ReadWaitingOffDayEditDto>();
		try
		{
			var resultData = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => p.ID == id && 
                p.Personal.Status == EntityStatusEnum.Online,
				include: p=> p.Include(a=>a.Personal)
											.Include(a=>a.Personal));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			if(resultData is null||branchList is null || positionList is null)
				return res.SetStatus(false).SetErr("OffDay Not Found").SetMessage("İlgili Personele Ait İzin Bulunamadı!!!");
			
			var mapData = _mapper.Map<ReadWaitingOffDayEditDto>(resultData);
				var branch = branchList.FirstOrDefault(b => b.ID == resultData.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == resultData.PositionId);
				if (branch is not null && position is not null)
				{
					mapData.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					mapData.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
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
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
						(!query.id.HasValue || a.Personal_Id == query.id) &&
						(!query.filterYear.HasValue || a.StartDate.Year == query.filterYear || a.EndDate.Year == query.filterYear) &&
						(!query.filterMonth.HasValue || a.StartDate.Month == query.filterMonth || a.EndDate.Month == query.filterMonth)&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.Contains(query.search))&&
						// (string.IsNullOrEmpty(query.branchName) || a.Personal.Branch.Name.Contains(query.branchName))&&
						// (string.IsNullOrEmpty(query.positionName) || a.Personal.Position.Name.Contains(query.positionName))&&
						(string.IsNullOrEmpty(query.isFreedayLeave) || a.LeaveByFreeDay > 0),
					include: p=>p.Include(a=>a.Personal).ThenInclude(a=>a.Branch).Include(a=>a.Personal.Position),
					orderBy: p =>
					{
						IOrderedQueryable<OffDay> orderedOffdays;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedOffdays = query.sortName switch
							{
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname)
									: p.OrderByDescending(a => a.Personal.NameSurname),
								"branchName" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.Branch.Name)
									: p.OrderByDescending(a => a.Personal.Branch.Name),
								"positionName" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.Position.Name)
									: p.OrderByDescending(a => a.Personal.Position.Name),
								"countLeave" => query.sortBy == "asc"
									? p.OrderBy(a => a.CountLeave)
									: p.OrderByDescending(a => a.CountLeave),
								"createdAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.CreatedAt)
									: p.OrderByDescending(a => a.CreatedAt),
								_ => p.OrderByDescending(a=> a.CreatedAt)
							};
						}
						else
						{
							orderedOffdays = p.OrderByDescending(a=> a.CreatedAt);
						}

						return orderedOffdays;
					}
				));
			
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			
			if(allData is null||branchList is null || positionList is null)
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("İzinler Bulunamadı...");
			if (!string.IsNullOrEmpty(query.branchName))
			{
				allData = allData.Where(a => branchList.Any(b => b.ID == a.BranchId && b.Name.Contains(query.branchName)));
			}
			if (!string.IsNullOrEmpty(query.positionName))
			{
				allData = allData.Where(a => positionList.Any(b => b.ID == a.PositionId && b.Name.Contains(query.positionName)));
			}
			var mapData = _mapper.Map<List<ReadApprovedOffDayListDto>>(allData);
			foreach (var item in mapData)
			{
				var branch = branchList.FirstOrDefault(b => b.ID == item.BranchId);
				var position = positionList.FirstOrDefault(p => p.ID == item.PositionId);
				if (branch is not null && position is not null)
				{
					item.BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name;
					item.PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name;
				}
			}
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