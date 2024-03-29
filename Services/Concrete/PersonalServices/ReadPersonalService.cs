﻿using System.Collections.Immutable;
using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.PassivePersonalDtos;
using Core.DTOs.PersonalDetailDto.ReadDtos;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.DTOs.PositionDTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.PersonalServices;

namespace Services.Concrete.PersonalServices;

public class ReadPersonalService : IReadPersonalService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadPersonalService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}
	
	public async Task<IResultWithDataDto<List<PersonalDto>>> GetExcelPersonalListService(PersonalQuery query)
	{
		IResultWithDataDto<List<PersonalDto>> res = new ResultWithDataDto<List<PersonalDto>>();
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname),
					predicate: a => (a.Status == EntityStatusEnum.Online) && 
					                (a.Branch.Status == EntityStatusEnum.Online || a.Branch.Status == EntityStatusEnum.Offline)&&
					                (a.Position.Status == EntityStatusEnum.Online || a.Position.Status == EntityStatusEnum.Offline)&&
					                (string.IsNullOrEmpty(query.search) || a.NameSurname.Contains(query.search))&& 
					                (string.IsNullOrEmpty(query.gender) || a.Gender.Contains(query.gender))&& 
					                (string.IsNullOrEmpty(query.branch) || a.Branch_Id.ToString().Contains(query.branch))&& 
					                (string.IsNullOrEmpty(query.position) || a.Position_Id.ToString().Contains(query.position))&& 
					                (string.IsNullOrEmpty(query.retired) || a.RetiredOrOld)
				)
			);   
			var resultData = allData.ToList();
			var mapData = _mapper.Map<List<PersonalDto>>(resultData);
			res.SetData(mapData);
			
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultWithDataDto<List<PassivePersonalDto>>> GetExcelPassivePersonalListService(PersonalQuery query)
	{
		IResultWithDataDto<List<PassivePersonalDto>> res = new ResultWithDataDto<List<PassivePersonalDto>>();
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname),
					predicate: a => (a.Status == EntityStatusEnum.Offline) && 
					                (a.Branch.Status == EntityStatusEnum.Online || a.Branch.Status == EntityStatusEnum.Offline)&&
					                (a.Position.Status == EntityStatusEnum.Online || a.Position.Status == EntityStatusEnum.Offline)&&
					                (string.IsNullOrEmpty(query.search) || a.NameSurname.Contains(query.search))&& 
					                (string.IsNullOrEmpty(query.gender) || a.Gender.Contains(query.gender))&& 
					                (string.IsNullOrEmpty(query.branch) || a.Branch_Id.ToString().Contains(query.branch))&& 
					                (string.IsNullOrEmpty(query.position) || a.Position_Id.ToString().Contains(query.position))&& 
					                (string.IsNullOrEmpty(query.retired) || a.RetiredOrOld)
				)
			);   
			var resultData = allData.ToList();
			var mapData = _mapper.Map<List<PassivePersonalDto>>(resultData);
			res.SetData(mapData);
			
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}


	public async Task<IResultWithDataDto<ReadUpdatePersonalDto>> GetUpdatePersonalService(Guid id)
	{
		IResultWithDataDto<ReadUpdatePersonalDto> res = new ResultWithDataDto<ReadUpdatePersonalDto>();
		try
		{
			var resultData = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate:p=>p.ID == id,
				include: p=> p.Include(a=>a.PersonalDetails)
			);
			var mapData = _mapper.Map<ReadUpdatePersonalDto>(resultData);
			mapData.Positions = await Task.Run(() => _unitOfWork.ReadPositionRepository
				.GetAll(predicate: p=> p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline,
					orderBy: o=> o.OrderBy(p=> p.Name))
				.Select(p => new PositionNameDto{ ID = p.ID, Name = p.Name })
				.ToList()
			);
			mapData.Branches = await Task.Run(() => _unitOfWork.ReadBranchRepository
				.GetAll(predicate: p=> p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline,
					orderBy: o=> o.OrderBy(p=> p.Name))
				.Select(p => new BranchNameDto{ ID = p.ID, Name = p.Name })
				.ToList()
			);
			
			res.SetData(mapData);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<ResultWithPagingDataDto<List<PersonalDto>>> GetPersonalListService(PersonalQuery query)
	{
		ResultWithPagingDataDto<List<PersonalDto>> res = new ResultWithPagingDataDto<List<PersonalDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname),
					predicate: a => (a.Status == EntityStatusEnum.Online) && 
					                (a.Branch.Status == EntityStatusEnum.Online || a.Branch.Status == EntityStatusEnum.Offline)&&
					                (a.Position.Status == EntityStatusEnum.Online || a.Position.Status == EntityStatusEnum.Offline)&&
					                (string.IsNullOrEmpty(query.search) || a.NameSurname.Contains(query.search))&& 
					                (string.IsNullOrEmpty(query.gender) || a.Gender.Contains(query.gender))&& 
					                (string.IsNullOrEmpty(query.branch) || a.Branch_Id.ToString().Contains(query.branch))&& 
					                (string.IsNullOrEmpty(query.position) || a.Position_Id.ToString().Contains(query.position))&& 
					                (string.IsNullOrEmpty(query.retired) || a.RetiredOrOld)
				)
				);   
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<PersonalDto>>(resultData);
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

	public async Task<ResultWithPagingDataDto<List<PassivePersonalDto>>> GetPassivePersonalListService(PersonalQuery query)
	{
		ResultWithPagingDataDto<List<PassivePersonalDto>> res = new ResultWithPagingDataDto<List<PassivePersonalDto>>(query.sayfa,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname),
					predicate: a => (a.Status == EntityStatusEnum.Offline) && 
					                (a.Branch.Status == EntityStatusEnum.Online || a.Branch.Status == EntityStatusEnum.Offline)&&
					                (a.Position.Status == EntityStatusEnum.Online || a.Position.Status == EntityStatusEnum.Offline)&&
					                (string.IsNullOrEmpty(query.search) || a.NameSurname.Contains(query.search))&& 
					                (string.IsNullOrEmpty(query.gender) || a.Gender.Contains(query.gender))&& 
					                (string.IsNullOrEmpty(query.branch) || a.Branch_Id.ToString().Contains(query.branch))&& 
					                (string.IsNullOrEmpty(query.position) || a.Position_Id.ToString().Contains(query.position))&& 
					                (string.IsNullOrEmpty(query.retired) || a.RetiredOrOld)
				)
			);   
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<PassivePersonalDto>>(resultData);
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

	public async Task<ResultWithPagingDataDto<List<PersonalDto>>> GetDeletedPersonalListService(int pageNumber, string search)
	{
		ResultWithPagingDataDto<List<PersonalDto>> res = new ResultWithPagingDataDto<List<PersonalDto>>(pageNumber, search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					orderBy: p => p.OrderByDescending(a => a.DeletedAt),
					predicate: a => (a.Status == EntityStatusEnum.Archive) && 
					                (string.IsNullOrEmpty(search) || a.NameSurname.Contains(search))
				));
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<PersonalDto>>(resultData);
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

	public async Task<IResultWithDataDto<List<ReadPersonalsByBranchIdDto>>> GetAllPersonalByBranchIdService(Guid branchId)
	{
		IResultWithDataDto<List<ReadPersonalsByBranchIdDto>> res = new ResultWithDataDto<List<ReadPersonalsByBranchIdDto>>();
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					predicate: a => 
									a.Branch_Id == branchId &&
									a.Status == EntityStatusEnum.Online && 
					                a.Branch.Status == EntityStatusEnum.Online&&
					                a.Position.Status == EntityStatusEnum.Online,
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname)
				)
			);   
			if(allData.IsNullOrEmpty()) 
				res.SetStatus(false).SetErr("Branch Is Not Found").SetMessage("Şubeye ait personeller bulunamadı.Lütfen sistemi kontrol ediniz!");
				
			var mappedResult = allData.Select(a => new ReadPersonalsByBranchIdDto
			{
				ID = a.ID,
				NameSurname = a.NameSurname,
				TotalYearLeave = a.TotalYearLeave,
				UsedYearLeave = a.UsedYearLeave,
				PositionName = a.Position.Name,
				BranchName = a.Branch.Name
			}).ToList();
			res.SetData(mappedResult);
			
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultWithDataDto<ReadPersonalDetailsHeaderDto>> GetPersonalDetailsHeaderByIdService(Guid id)
	{
		IResultWithDataDto<ReadPersonalDetailsHeaderDto> res = new ResultWithDataDto<ReadPersonalDetailsHeaderDto>();
		try
		{
			var personel = await _unitOfWork.ReadPersonalRepository.GetSingleAsync(predicate: p => p.ID == id,
				include:p=>p.Include(a=>a.Branch).Include(a=>a.Position));
			if(personel is null)
				return res.SetStatus(false).SetErr("Personel Is Not Found").SetMessage("İlgili Personel bulunamadı.Lütfen sistemi kontrol ediniz!");
			var mappedResult = _mapper.Map<ReadPersonalDetailsHeaderDto>(personel);
			res.SetData(mappedResult);
		}
		catch (Exception e)
		{
			res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}
}