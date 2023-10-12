using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalDTOs;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs;

public class ReadOffDayDto : ReadBaseDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public OffDayStatusEnum OffDayStatus { get; set; }
    public int Personal_Id { get; set; }
    public ReadPersonalDto Personal { get; set; }
}