using Core.Enums;


namespace Core.DTOs.BaseDTOs;

public class WriteBaseDto : BaseDto
{
	public int ID { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }
	public DateTime DeletedAt { get; set; }
	public EntityStatusEnum Status { get; set; }
}