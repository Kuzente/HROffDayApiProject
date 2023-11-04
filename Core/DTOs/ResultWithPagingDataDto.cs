using Core.Interfaces;

namespace Core.DTOs;

public class ResultWithPagingDataDto<T> : ResultWithDataDto<T> 
{
    public ResultWithPagingDataDto()
    {
        this.PageNumber = 1;
        this.PageSize = 8;
    }
    public ResultWithPagingDataDto(int pageNumber,string searchParam)
    {
        this.PageNumber = pageNumber;
        this.PageSize  = 8;
        this.SearchParams = searchParam;
    }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public string SearchParams { get; set; }
   
}