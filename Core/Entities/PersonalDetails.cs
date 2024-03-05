using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class PersonalDetails : BaseEntity
{
   
    public string BirthPlace { get; set; }
    public string? BloodGroup { get; set; }
    public string? MaritalStatus { get; set; }
    public bool Handicapped { get; set; }
    public string? EducationStatus { get; set; }
    public string? PersonalGroup { get; set; }
    public double Salary { get; set; }
    public string? SgkCode { get; set; }
    public string SskNumber { get; set; }
    public string? WorkingPlace { get; set; }
    public string? Address { get; set; }
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? BodySize { get; set; }
    public string? IBAN { get; set; }
    public string? BankAccount { get; set; }
    [Required]
    public Guid Personal_Id { get; set; }
    [ForeignKey(nameof(Personal_Id))]
    public Personal Personal { get; set; }
}