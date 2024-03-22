﻿using Core.DTOs.BaseDTOs;
using Core.DTOs.PersonalDetailDto.WriteDtos;

namespace Core.DTOs.PersonalDTOs;

public class AddRangePersonalDto : BaseDto
{
    public string NameSurname { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime StartJobDate { get; set; }
    public string IdentificationNumber { get; set; }
    public int RegistirationNumber { get; set; }
    public string Phonenumber { get; set; }
    public bool RetiredOrOld { get; set; }
    public DateTime? RetiredDate { get; set; }
    public int FoodAid { get; set; }
    public DateTime FoodAidDate { get; set; }
        
    public string Gender { get; set; }
    public int TotalTakenLeave { get; set; }
    public int TotalYearLeave { get; set; }
    public int UsedYearLeave { get; set; } 
    public Guid Branch_Id { get; set; }
    public Guid Position_Id { get; set; }
    public AddRangePersonalDetailDto PersonalDetails { get; set; }
}