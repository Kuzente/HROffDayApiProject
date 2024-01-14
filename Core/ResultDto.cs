using Core.Interfaces;

namespace Core;

public class ResultDto : IResultDto
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "OK";
    public string Err { get; set; } = "OK";
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