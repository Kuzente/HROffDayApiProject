using Core.Interfaces;

namespace Core.DTOs;

public class ResultWithPagingDataDto<T> : ResultWithDataDto<T> 
{
    public ResultWithPagingDataDto(int pageNumber = 1, int pageSize = 10)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
   
}