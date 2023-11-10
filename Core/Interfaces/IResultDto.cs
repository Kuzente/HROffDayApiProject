namespace Core.Interfaces;

public interface IResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string Err { get; set; }
    public IResultDto SetMessage(string message = "OK");
    public IResultDto SetErr(string err = "OK");
    public IResultDto SetStatus(bool isSuccess = true);
}