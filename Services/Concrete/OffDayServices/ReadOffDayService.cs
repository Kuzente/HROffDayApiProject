﻿using System.Linq.Expressions;
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
using Microsoft.IdentityModel.Tokens;
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
		DateTime? filterStartDate = null;
		DateTime? filterEndDate = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(query.filterDate))
			{
				var dates = query.filterDate.Split(" ile ");
				if (dates.Length == 2)
				{
					filterStartDate = DateTime.Parse(dates[0].Trim());
					filterEndDate = DateTime.Parse(dates[1].Trim());
				}
			}
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online &&
						 a.OffDayStatus == OffDayStatusEnum.WaitingForFirst) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(string.IsNullOrWhiteSpace(query.filterDate) ||
						(filterStartDate.HasValue && filterEndDate.HasValue &&
						a.StartDate <= filterEndDate.Value && a.EndDate >= filterStartDate.Value))
						&&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
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
		DateTime? filterStartDate = null;
		DateTime? filterEndDate = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(query.filterDate))
			{
				var dates = query.filterDate.Split(" ile ");
				if (dates.Length == 2)
				{
					filterStartDate = DateTime.Parse(dates[0].Trim());
					filterEndDate = DateTime.Parse(dates[1].Trim());
				}
			}
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && 
						 a.OffDayStatus == OffDayStatusEnum.WaitingForSecond) &&
						query.UserBranches.Any(q=>q == a.BranchId) &&
						a.Personal.Status == EntityStatusEnum.Online &&
						(string.IsNullOrWhiteSpace(query.filterDate) ||
						(filterStartDate.HasValue && filterEndDate.HasValue &&
						a.StartDate <= filterEndDate.Value && a.EndDate >= filterStartDate.Value)) &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
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
		DateTime? filterStartDate = null;
		DateTime? filterEndDate = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(query.filterDate))
			{
				var dates = query.filterDate.Split(" ile ");
				if (dates.Length == 2)
				{
					filterStartDate = DateTime.Parse(dates[0].Trim());
					filterEndDate = DateTime.Parse(dates[1].Trim());
				}
			}
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						a.Status == EntityStatusEnum.Online &&
						 a.OffDayStatus == OffDayStatusEnum.Rejected &&
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
						(query.UserBranches.IsNullOrEmpty()|| query.UserBranches.Any(q=>q == a.BranchId)) &&
						(string.IsNullOrWhiteSpace(query.filterDate) ||
						(filterStartDate.HasValue && filterEndDate.HasValue &&
						a.StartDate <= filterEndDate.Value && a.EndDate >= filterStartDate.Value)) &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
					include: p=>p.Include(a=>a.Personal),
					orderBy: p => p.OrderByDescending(a => a.CreatedAt)
				));
			var branchList = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll().Select(a=> new { a.Name,a.ID , a.Status}));
			var positionList = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll().Select(a=> new { a.Name,a.ID, a.Status}));
			if(allData is null ||branchList.IsNullOrEmpty() || positionList.IsNullOrEmpty())
				res.SetStatus(false).SetErr("Branch or Position or OffDay is not found").SetMessage("Şube veya Ünvan Bulunamadı...");
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
		DateTime? filterStartDate = null;
		DateTime? filterEndDate = null;
		ReadApprovedOffDayDto mapDataDto = new()
		{
			ReadApprovedOffDayListDtos = new(),
			ReadApprovedOffDayGetBranchesList = new(),
			ReadApprovedOffDayGetPositionsList = new()
		};
		try
		{
			if (!string.IsNullOrWhiteSpace(query.filterDate))
			{
				var dates = query.filterDate.Split(" ile ");
				if (dates.Length == 2)
				{
					filterStartDate = DateTime.Parse(dates[0].Trim());
					filterEndDate = DateTime.Parse(dates[1].Trim());
				}
			}
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						a.Status == EntityStatusEnum.Online && // İzin silinmemiş ise
						a.OffDayStatus == OffDayStatusEnum.Approved  && // İzin onaylanmış ise
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) && // İzne ait personel çalışan veya işten çıkarılmış ise
						(query.UserBranches.IsNullOrEmpty() || query.UserBranches.Any(q=>q == a.BranchId)) && //Sisteme giren kullanıcıya ait atanmış şubeler var ise
						(string.IsNullOrWhiteSpace(query.filterDate) ||
						(filterStartDate.HasValue && filterEndDate.HasValue &&
						a.StartDate <= filterEndDate.Value && a.EndDate >= filterStartDate.Value)) &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower()))&& // Filtre üzerinde arama yapılmış ise
						(string.IsNullOrEmpty(query.positionName) || a.Personal.Position.ID.ToString() == query.positionName)&& // Filtre üzerinde ünvan seçili ise
						(string.IsNullOrEmpty(query.branchName) || a.Personal.Branch.ID.ToString() == query.branchName)&& // Filtre üzerinde şube seçili ise
						(string.IsNullOrEmpty(query.isFreedayLeave) || a.LeaveByFreeDay > 0), // Filtre üzerinde ücretsiz izin seçili ise
					
					include: p=>
						p.Include(a=>a.Personal)
							.ThenInclude(b=> b.Branch)
							.Include(a=>a.Personal)
							.ThenInclude(b=>b.Position),
					orderBy: p =>
					{
						IOrderedQueryable<OffDay> orderedOffdays;
						if (query.sortName is not null && query.sortBy is not null)
						{
							orderedOffdays = query.sortName switch
							{
								"documentNumber" => query.sortBy == "asc"
									? p.OrderBy(a => a.DocumentNumber)
									: p.OrderByDescending(a => a.DocumentNumber),
								"nameSurname" => query.sortBy == "asc"
									? p.OrderBy(a => a.Personal.NameSurname)
									: p.OrderByDescending(a => a.Personal.NameSurname),
								"countLeave" => query.sortBy == "asc"
									? p.OrderBy(a => a.CountLeave)
									: p.OrderByDescending(a => a.CountLeave),
								"createdAt" => query.sortBy == "asc"
									? p.OrderBy(a => a.CreatedAt)
									: p.OrderByDescending(a => a.CreatedAt),
								"branchName" => query.sortBy == "asc"
									? p.OrderBy(a=>a.Personal.Branch.Name)
									: p.OrderByDescending(a=>a.Personal.Branch.Name),
								"positionName" => query.sortBy == "asc"
									? p.OrderBy(a=>a.Personal.Position.Name)
									: p.OrderByDescending(a=>a.Personal.Position.Name),
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
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			mapDataDto.ReadApprovedOffDayListDtos = _mapper.Map<List<ReadApprovedOffDayListDto>>(resultData);
			var branchListquery = await Task.Run(() => _unitOfWork.ReadBranchRepository
				.GetAll(
					predicate:p=> (query.UserBranches.IsNullOrEmpty() || query.UserBranches.Any(b=> b == p.ID)),
					orderBy:o=> o.OrderBy(p=> p.Name))
				.Select(a=> new { a.Name,a.ID , a.Status}));
			
			foreach (var branch in branchListquery)
			{
				mapDataDto.ReadApprovedOffDayGetBranchesList.Add(new ReadApprovedOffDayGetBranches
				{
					BranchName = branch.Status == EntityStatusEnum.Archive ? $"{branch.Name} (Silinmiş)" : branch.Name,
					BranchId = branch.ID,
				});
			}
			var positionListquery = await Task.Run(() => _unitOfWork.ReadPositionRepository
				.GetAll(orderBy:o=> o.OrderBy(p=> p.Name))
				.Select(a=> new { a.Name,a.ID, a.Status}));
			foreach (var position in positionListquery)
			{
				mapDataDto.ReadApprovedOffDayGetPositionsList.Add(new ReadApprovedOffDayGetPositions
				{
					PositionName = position.Status == EntityStatusEnum.Archive ? $"{position.Name} (Silinmiş)" : position.Name,
					PositionId = position.ID,
				});
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
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower())),
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
		DateTime? filterStartDate = null;
		DateTime? filterEndDate = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(query.filterDate))
			{
				var dates = query.filterDate.Split(" ile ");
				if (dates.Length == 2)
				{
					filterStartDate = DateTime.Parse(dates[0].Trim());
					filterEndDate = DateTime.Parse(dates[1].Trim());
				}
			}
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.Approved) &&
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) && a.Personal_Id == query.id &&
						(string.IsNullOrWhiteSpace(query.filterDate) ||
						(filterStartDate.HasValue && filterEndDate.HasValue &&
						a.StartDate <= filterEndDate.Value && a.EndDate >= filterStartDate.Value)),
					include: p=> p.Include(a=>a.Personal),
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

	public async Task<IResultWithDataDto<ReadWaitingOffDayEditDto>> GetOffDayEditService(Guid id)
	{
		IResultWithDataDto<ReadWaitingOffDayEditDto> res = new ResultWithDataDto<ReadWaitingOffDayEditDto>();
		try
		{
			var getOffDay = await _unitOfWork.ReadOffDayRepository.GetSingleAsync(
				predicate: p => 
						p.ID == id && 
						p.Personal.Status == EntityStatusEnum.Online,
				include: p=> p
						.Include(a=>a.Personal)
				);
			if(getOffDay is null)
				return res.SetStatus(false).SetErr("OffDay Not Found").SetMessage("İlgili Personele Ait İzin Bulunamadı!!!");
			//Burada Şube ve Ünvanı Ekstra olarak almamızın sebebi OffDay üzerine hangi şube veya ünvan ile işlem yapıldıysa onu yakalamak için
			var offDayBranch = await _unitOfWork.ReadBranchRepository.GetSingleAsync(predicate:p=> p.ID == getOffDay.BranchId);
			var offDayPosition = await _unitOfWork.ReadPositionRepository.GetSingleAsync(predicate:p=> p.ID == getOffDay.PositionId);
			if(offDayBranch is null || offDayPosition is null)
				return res.SetStatus(false).SetErr("Branch or Position Not Found").SetMessage("İlgili Personele Şube veya Ünvan Bulunamadı!!!");
			var mapData = _mapper.Map<ReadWaitingOffDayEditDto>(getOffDay);
			mapData.BranchName = offDayBranch.Status == EntityStatusEnum.Archive ? $"{offDayBranch.Name} (Silinmiş)" : offDayBranch.Name;
			mapData.PositionName = offDayPosition.Status == EntityStatusEnum.Archive ? $"{offDayPosition.Name} (Silinmiş)" : offDayPosition.Name;
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
		DateTime? filterStartDate = null;
		DateTime? filterEndDate = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(query.filterDate))
			{
				var dates = query.filterDate.Split(" ile ");
				if (dates.Length == 2)
				{
					filterStartDate = DateTime.Parse(dates[0].Trim());
					filterEndDate = DateTime.Parse(dates[1].Trim());
				}
			}
			var allData = await Task.Run(() =>
				_unitOfWork.ReadOffDayRepository.GetAll(
					predicate: a =>
						(a.Status == EntityStatusEnum.Online && a.OffDayStatus == OffDayStatusEnum.Approved ) &&
						(a.Personal.Status == EntityStatusEnum.Online || a.Personal.Status == EntityStatusEnum.Offline) &&
						(query.UserBranches.IsNullOrEmpty() || query.UserBranches.Any(q=>q == a.BranchId)) &&
						(!query.id.HasValue || a.Personal_Id == query.id) &&
						(string.IsNullOrWhiteSpace(query.filterDate) ||
						(filterStartDate.HasValue && filterEndDate.HasValue &&
						a.StartDate <= filterEndDate.Value && a.EndDate >= filterStartDate.Value)) &&
						(string.IsNullOrEmpty(query.search) || a.Personal.NameSurname.ToLower().Contains(query.search.ToLower()))&&
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
					include: p=> p.Include(a=> a.Personal)
			);
			if (offday is null) return res.SetStatus(false).SetErr("Offday is not find").SetMessage("İlgili izin bulunamadı!");
			var mapData = _mapper.Map<ReadApprovedOffDayFormExcelExportDto>(offday);
			var branchquery = await _unitOfWork.ReadBranchRepository.GetSingleAsync(predicate:p=>p.ID == offday.BranchId);
			var positionquery = await _unitOfWork.ReadPositionRepository.GetSingleAsync(predicate:p=>p.ID == offday.PositionId);
			if (branchquery is null || positionquery is null) res.SetStatus(false).SetErr("Offday is not find").SetMessage("İlgili izin bulunamadı!");
			mapData.BranchName = branchquery.Name;
			mapData.PositionName = positionquery.Name;
			res.SetData(mapData);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	
}