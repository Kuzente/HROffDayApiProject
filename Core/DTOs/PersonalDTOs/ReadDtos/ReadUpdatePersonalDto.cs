using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDetailDto.ReadDtos;
using Core.DTOs.PositionDTOs;
using Core.Enums;

namespace Core.DTOs.PersonalDTOs.ReadDtos;

public class ReadUpdatePersonalDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime StartJobDate { get; set; }
    public DateTime? EndJobDate { get; set; } 
    public string IdentificationNumber { get; set; }
    public int RegistirationNumber { get; set; }
    public string Phonenumber { get; set; }
    public bool RetiredOrOld { get; set; }
    public DateTime RetiredDate { get; set; }
    public DateTime FoodAidDate { get; set; }
    public string Gender { get; set; }
    public int TotalYearLeave { get; set; }
    public double TotalTakenLeave { get; set; }
    public int UsedYearLeave { get; set; }
    public int FoodAid { get; set; }
    public Guid Branch_Id { get; set; }
    public Guid Position_Id { get; set; }
    public List<BranchNameDto> Branches { get; set; }
    public List<PositionNameDto> Positions { get; set; }
    public ReadUpdatePersonalDetailsDto PersonalDetails { get; set; }
    public EntityStatusEnum Status { get; set; }
    public bool IsBackToWork { get; set; }
    public DateTime YearLeaveDate { get; set; }
    public bool IsYearLeaveRetired { get; set; }
}