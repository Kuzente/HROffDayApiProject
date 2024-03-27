using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class BranchUser : BaseEntity
{
        [Required]
        public Guid UserID { get; set; }
    
        public User User { get; set; }
        [Required]
    
        public Guid BranchID { get; set; }
    
        public Branch Branch { get; set; }
}