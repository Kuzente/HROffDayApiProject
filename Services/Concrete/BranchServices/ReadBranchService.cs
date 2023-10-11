using AutoMapper;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Data.Abstract;
using Services.Abstract.BranchServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete.BranchServices
{
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
			ReadBranchDto result = new ReadBranchDto();
			var entities = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll());
			return _mapper.Map<List<ReadBranchDto>>(entities.ToList());
		}

		public Task<bool> GetAnyAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ReadBranchDto> GetSingleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
