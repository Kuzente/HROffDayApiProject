using AutoMapper;
using Core.DTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Data.Abstract.TransferPersonalRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.TransferPersonalService;

namespace Services.Concrete.TransferPersonalService;

public class ReadTransferPersonalService : IReadTransferPersonalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReadTransferPersonalService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResultWithPagingDataDto<List<ReadTransferPersonalDto>>> GetTransferPersonalListByIdService(TransferPersonalQuery query)
    {
        ResultWithPagingDataDto<List<ReadTransferPersonalDto>> res = new ResultWithPagingDataDto<List<ReadTransferPersonalDto>>(query.sayfa, query.search);
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadTransferPersonalRepository.GetAll(
                    predicate: a =>
                        a.Personal_Id == query.id &&
                        (a.Status == EntityStatusEnum.Online) &&
                        (!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
                        (!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth ),
                    orderBy: p => p.OrderByDescending(a => a.CreatedAt)
                ));
            var allDataMapped = allData.Select(a => new ReadTransferPersonalDto
            {
                ID = a.ID,
                OldBranch = a.OldBranch,
                NewBranch = a.NewBranch,
                NewPosition = a.NewPosition,
                OldPosition = a.OldPosition,
                Personal_Id = a.Personal_Id,
                PersonalNameSurname = a.Personal.NameSurname,
                CreatedAt = a.CreatedAt
            });
            var resultData = allDataMapped.Skip((res.PageNumber - 1) * res.PageSize)
                .Take(res.PageSize).ToList();
            res.SetData(resultData);
            res.TotalRecords = allData.Count();
            res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
        }
        catch (Exception e)
        {
            res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

    public async Task<IResultWithDataDto<List<ReadTransferPersonalDto>>> ExcelGetTransferPersonalListByIdService(TransferPersonalQuery query)
    {
        IResultWithDataDto<List<ReadTransferPersonalDto>> res = new ResultWithDataDto<List<ReadTransferPersonalDto>>();
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadTransferPersonalRepository.GetAll(
                    predicate: a =>
                        a.Personal_Id == query.id &&
                        (a.Status == EntityStatusEnum.Online) &&
                        (!query.filterYear.HasValue || a.CreatedAt.Year == query.filterYear) &&
                        (!query.filterMonth.HasValue || a.CreatedAt.Month == query.filterMonth ),
                    orderBy: p => p.OrderByDescending(a => a.CreatedAt)
                ));
            var allDataMapped = allData.Select(a => new ReadTransferPersonalDto
            {
                ID = a.ID,
                OldBranch = a.OldBranch,
                NewBranch = a.NewBranch,
                NewPosition = a.NewBranch,
                OldPosition = a.OldBranch,
                Personal_Id = a.Personal_Id,
                PersonalNameSurname = a.Personal.NameSurname,
                CreatedAt = a.CreatedAt
            }).ToList();
            res.SetData(allDataMapped);
        }
        catch (Exception e)
        {
            res.SetStatus(false).SetErr(e.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }
}