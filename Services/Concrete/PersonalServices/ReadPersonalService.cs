using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
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

	public async Task<List<PersonalDto>> GetAllAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll());
		return _mapper.Map<List<PersonalDto>>(entities.ToList());
	}

	public async Task<List<PersonalDto>> GetAllWithBranchAndPositionAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll(
			include:p=> p
			.Include(a=>a.Branch)
			.Include(b=>b.Position),
			orderBy: o=> o
			.OrderBy(o=>o.NameSurname)
			)
		);
		return _mapper.Map<List<PersonalDto>>(entities.ToList());
	}

	public async Task<IResultWithDataDto<List<PersonalDto>>> GetAllWithFilterAsync(PersonalQuery query)
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
					predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
					                (string.IsNullOrEmpty(query.search) || a.NameSurname.Contains(query.search))&& 
					                (string.IsNullOrEmpty(query.gender) || a.Gender.Contains(query.gender))&& 
					                (string.IsNullOrEmpty(query.branch) || a.Branch_Id.ToString().Contains(query.branch))&& 
					                (string.IsNullOrEmpty(query.position) || a.Position_Id.ToString().Contains(query.position))&& 
					                (string.IsNullOrEmpty(query.retired) || a.RetiredOrOld) && 
					                (string.IsNullOrEmpty(query.passive) || a.Status == EntityStatusEnum.Offline)
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


	public async Task<IResultWithDataDto<PersonalDetailDto>> GetByIdDetailedPersonal(int id)
	{
		IResultWithDataDto<PersonalDetailDto> res = new ResultWithDataDto<PersonalDetailDto>();
		try
		{
			var resultData = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetByIdAsync(id));
			var mapData = _mapper.Map<PersonalDetailDto>(resultData.FirstOrDefault());
			res.SetData(mapData);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<ResultWithPagingDataDto<List<PersonalDto>>> GetAllPagingWithBranchAndPositionOrderByAsync(PersonalQuery query)
	{
		ResultWithPagingDataDto<List<PersonalDto>> res = new ResultWithPagingDataDto<List<PersonalDto>>(query.pageNumber,query.search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname),
					predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
					                (string.IsNullOrEmpty(query.search) || a.NameSurname.Contains(query.search))&& 
					                (string.IsNullOrEmpty(query.gender) || a.Gender.Contains(query.gender))&& 
					                (string.IsNullOrEmpty(query.branch) || a.Branch_Id.ToString().Contains(query.branch))&& 
					                (string.IsNullOrEmpty(query.position) || a.Position_Id.ToString().Contains(query.position))&& 
					                (string.IsNullOrEmpty(query.retired) || a.RetiredOrOld) && 
					                (string.IsNullOrEmpty(query.passive) || a.Status == EntityStatusEnum.Offline)
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

	public async Task<ResultWithPagingDataDto<List<PersonalDto>>> GetAllDeletedPersonalPagingOrderByAsync(int pageNumber, string search)
	{
		ResultWithPagingDataDto<List<PersonalDto>> res = new ResultWithPagingDataDto<List<PersonalDto>>(pageNumber, search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					orderBy: p => p.OrderBy(a => a.NameSurname),
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

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException(); //TODO
	}

	public Task<PersonalDto> GetSingleAsync()
	{
		throw new NotImplementedException(); //TODO
	}

	public Task<bool> GetAnyAsync(Expression<Func<PersonalDto, bool>>? predicate = null)
	{
		throw new NotImplementedException();
	}
}