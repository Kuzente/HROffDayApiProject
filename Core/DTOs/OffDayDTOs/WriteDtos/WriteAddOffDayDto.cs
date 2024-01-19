﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.DTOs.OffDayDTOs.WriteDtos;

public class WriteAddOffDayDto
{
    [Required]
    public Guid Personal_Id { get; set; }
    [Required]
    public int CountLeave { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    [Required]
    public int LeaveByYear { get; set; }
    [Required]
    public int LeaveByWeek { get; set; }
    [Required]
    public int LeaveByTaken { get; set; }
    [Required]
    public int LeaveByPublicHoliday { get; set; }
    [Required]
    public int LeaveByFreeDay { get; set; }
    [Required]
    public int LeaveByTravel { get; set; }
    
    public List<string>? LeaveByMarriedFatherDead { get; set; }

    public string returnUrl { get; set; }
}