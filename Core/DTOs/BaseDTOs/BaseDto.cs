using Core.Enums;
using Core.Interfaces.Base;


namespace Core.DTOs.BaseDTOs;

public class BaseDto : IBaseDto
{
    public Guid ID { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public EntityStatusEnum Status { get; set; }
}