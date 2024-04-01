namespace Core.DTOs.MissingDayDtos.ReadDtos;

public class ReadMissingDayDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public string IdentificationNumber { get; set; }
    public DateTime StartOffdayDate { get; set; }
    public DateTime EndOffDayDate { get; set; }
    public DateTime? StartJobDate { get; set; }
    public string Reason { get; set; }
    public Guid Branch_Id { get; set; }
    public string BranchName { get; set; }
    public DateTime CreatedAt { get; set; }
}