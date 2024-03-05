using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.PersonalDetailDto.WriteDtos;

public class WriteUpdatePersonalDetailDto
{
    public Guid ID { get; set; }
    [Required]
    public string BirthPlace { get; set; }
    public string? BloodGroup { get; set; }
    public string? MaritalStatus { get; set; }
    public bool Handicapped { get; set; }
    public string? EducationStatus { get; set; }
    public string? PersonalGroup { get; set; }
    [Required]
    public double Salary { get; set; }
    [Required]
    public string? SgkCode { get; set; }
    [Required]
    public string SskNumber { get; set; }
    public string? WorkingPlace { get; set; }
    public string? Address { get; set; }
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? BodySize { get; set; }
    public string? IBAN { get; set; }
    public string? BankAccount { get; set; }
}