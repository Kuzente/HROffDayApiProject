using Core.DTOs.BaseDTOs;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs;

public class WriteOffDayDto :  WriteBaseDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public OffDayStatusEnum OffDayStatus { get; set; }
    public int Personal_Id { get; set; }
}