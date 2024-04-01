using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.MissingDayDtos.WriteDtos;

public class WriteAddMissingDayDto
{
    [Required]
    public Guid PersonalId { get; set; }
    [Required]
    public string Reason { get; set; }
    [Required]
    public DateTime StartOffdayDate { get; set; }
    [Required]
    public DateTime EndOffDayDate { get; set; }
    public DateTime? StartJobDate { get; set; }
}