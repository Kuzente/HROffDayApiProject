using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.PassivePersonalDtos;
using Core.Enums;
using Core.Interfaces;
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
	
	public async Task<List<ReadOffDayDto>> GetAllWithPersonal()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadOffDayRepository.GetAll(include:p=> p.Include(a=>a.Personal)));
		return _mapper.Map<List<ReadOffDayDto>>(entities.ToList());
	}

	
}