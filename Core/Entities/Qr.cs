using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Qr : BaseEntity
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public ICollection<Personal> Personals { get; set; }
    
}