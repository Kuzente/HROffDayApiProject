using Core.DTOs.BaseDTOs;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs;

public  class WriteOffDayDto : BaseDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public OffDayStatusEnum OffDayStatus { get; set; }
    public Guid Personal_Id { get; set; }
}
