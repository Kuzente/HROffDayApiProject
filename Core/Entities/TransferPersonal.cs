using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class TransferPersonal : BaseEntity
{
    [Required]
    public string OldBranch { get; set; }
    [Required]
    public string NewBranch { get; set; }
    [Required]
    public string OldPosition { get; set; }
    [Required]
    public string NewPosition { get; set; }
    [Required]
    public Guid Personal_Id { get; set; }
    [ForeignKey(nameof(Personal_Id))]
    public Personal Personal { get; set; }
}