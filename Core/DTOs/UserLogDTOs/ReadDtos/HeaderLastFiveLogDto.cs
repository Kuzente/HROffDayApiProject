using Core.Enums;

namespace Core.DTOs.UserLogDTOs.ReadDtos;

public class HeaderLastFiveLogDto
{
    public DateTime CreatedAt { get; set; }
    public LogType LogType { get; set; }
    public string Description { get; set; }
    public string Username { get; set; }
}