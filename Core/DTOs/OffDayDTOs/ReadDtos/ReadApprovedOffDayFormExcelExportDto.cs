namespace Core.DTOs.OffDayDTOs.ReadDtos;

public class ReadApprovedOffDayFormExcelExportDto
{
    public Guid ID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CountLeave { get; set; }
    public int LeaveByWeek { get; set; }
    public int LeaveByYear { get; set; } 
    public int LeaveByPublicHoliday { get; set; } 
    public int LeaveByFreeDay { get; set; } 
    public int LeaveByTaken { get; set; } 
    public int LeaveByTravel { get; set; } 
    public int LeaveByDead { get; set; } 
    public int LeaveByFather { get; set; }
    public int LeaveByMarried { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public Guid Personal_Id { get; set; }
    public ReadApprovedOffDayFormExcelExportSubPersonalDto Personal { get; set; }
}

public class ReadApprovedOffDayFormExcelExportSubPersonalDto
{
    public string NameSurname { get; set; }
    public string IdentificationNumber { get; set; }
    public string RegistirationNumber { get; set; }
    public int UsedYearLeave { get; set; }
    public int TotalYearLeave { get; set; }
    public ReadApprovedOffDayFormExcelExportSubPersonalSubBranchDto Branch { get; set; }
    public ReadApprovedOffDayFormExcelExportSubPersonalSubPositionDto Position { get; set; }
}

public class ReadApprovedOffDayFormExcelExportSubPersonalSubBranchDto
{
    public string Name { get; set; }
}
public class ReadApprovedOffDayFormExcelExportSubPersonalSubPositionDto
{
    public string Name { get; set; }
}