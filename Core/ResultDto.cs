using Core.Interfaces;

namespace Core;

public class ResultDto : IResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string Err { get; set; }
    public IResultDto SetErr(string err)
    {
        Err = err;
        return this;
    }
    public IResultDto SetMessage(string message)
    {
        Message = message;
        return this;
    }
    public IResultDto SetStatus(bool statusValue = true)
    {
        IsSuccess = statusValue;
        return this;
    }
}