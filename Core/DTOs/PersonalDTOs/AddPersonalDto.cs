using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.PersonalDetailDto.WriteDtos;

namespace Core.DTOs.PersonalDTOs;

public class AddPersonalDto : BaseDto
{
    [Required]
    public string NameSurname { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public DateTime StartJobDate { get; set; }
    [Required]
    public string IdentificationNumber { get; set; }
    [Required]
    public int RegistirationNumber { get; set; }
    public string? Phonenumber { get; set; }
    public bool RetiredOrOld { get; set; }
    public DateTime? RetiredDate { get; set; }
    [Required]
    public string Gender { get; set; }
    public int TotalYearLeave { get; set; }
    public int UsedYearLeave { get; set; } 
    [Required]
    public Guid Branch_Id { get; set; }
    [Required]
    public Guid Position_Id { get; set; }
    public AddPersonalDetailDto PersonalDetails { get; set; }
       
}


