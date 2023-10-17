using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs.PersonalDTOs;
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

	public async Task<List<ReadPersonalDto>> GetAllAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll());
		return _mapper.Map<List<ReadPersonalDto>>(entities.ToList());
	}

	public async Task<List<ReadPersonalDto>> GetAllWithBranchAndPositionAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll(include:p=> p.Include(a=>a.Branch).Include(b=>b.Position)));
		return _mapper.Map<List<ReadPersonalDto>>(entities.ToList());
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException(); //TODO
	}

	public Task<ReadPersonalDto> GetSingleAsync()
	{
		throw new NotImplementedException(); //TODO
	}

	public Task<bool> GetAnyAsync(Expression<Func<ReadPersonalDto, bool>>? predicate = null)
	{
		throw new NotImplementedException();
	}
}