using System.ComponentModel.DataAnnotations;
using Core.DTOs.PersonalDetailDto.WriteDtos;

namespace Core.DTOs.PersonalDTOs.WriteDtos;

public class WriteUpdatePersonalDto
{
    [Required]
    public Guid ID { get; set; }
    [Required]
    public string NameSurname { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public DateTime StartJobDate { get; set; }
    [Required]
    public string IdentificationNumber { get; set; }
    [Required]
    public string RegistirationNumber { get; set; }
    public string? Phonenumber { get; set; }
    public bool RetiredOrOld { get; set; }
    public DateTime? RetiredDate { get; set; }
    [Required]
    public double TotalTakenLeave { get; set; }
    [Required]
    public string Gender { get; set; }
    [Required]
    public int TotalYearLeave { get; set; }
    [Required]
    public int UsedYearLeave { get; set; } 
    [Required] 
    public int FoodAid { get; set; }
    [Required] 
    public DateTime FoodAidDate { get; set; }
    [Required]
    public Guid Branch_Id { get; set; }
    [Required]
    public Guid Position_Id { get; set; }
    public WriteUpdatePersonalDetailDto PersonalDetails { get; set; }
}