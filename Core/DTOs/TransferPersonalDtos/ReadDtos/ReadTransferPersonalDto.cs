namespace Core.DTOs.TransferPersonalDtos.ReadDtos;

public class ReadTransferPersonalDto
{
    public Guid ID { get; set; }
    public string OldBranch { get; set; }
    public string NewBranch { get; set; }
    public string OldPosition { get; set; }
    public string NewPosition { get; set; }
    public Guid Personal_Id { get; set; }
    public string PersonalNameSurname { get; set; }
    public DateTime CreatedAt { get; set; }
    
}