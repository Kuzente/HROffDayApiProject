﻿using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;

namespace Services.Abstract.BranchServices;

public interface IReadBranchService : IReadService<BranchDto>
{
	Task<IResultWithDataDto<List<BranchDto>>> GetAllOrderByAsync();
	Task<ResultWithPagingDataDto<List<BranchDto>>> GetAllPagingOrderByAsync(int pageNumber,string search, bool passive);
	Task<ResultWithPagingDataDto<List<BranchDto>>> GetAllDeletedBranchPagingOrderByAsync(int pageNumber,string search);
	Task<IResultWithDataDto<BranchDto>> GetByIdUpdate(int id);
    Task<List<BranchNameDto>> GetAllJustNames();


}