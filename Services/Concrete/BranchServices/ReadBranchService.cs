using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs.BranchDTOs;
using Data.Abstract;
using Services.Abstract.BranchServices;

namespace Services.Concrete.BranchServices;

public class ReadBranchService : IReadBranchService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadBranchService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}
	public async Task<List<ReadBranchDto>> GetAllAsync()
	{			
		var entities = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll());
		return _mapper.Map<List<ReadBranchDto>>(entities.ToList());
	}

	public Task<ReadBranchDto> GetSingleAsync()
	{
		throw new NotImplementedException(); 
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<List<ReadBranchDto>> GetAllOrderByAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll(orderBy: p=> p.OrderBy(a=>a.Name)));
		return _mapper.Map<List<ReadBranchDto>>(entities.ToList());
	}
}