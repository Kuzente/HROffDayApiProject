﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class MissingDay : BaseEntity
{
    [Required]
    public DateTime StartOffdayDate { get; set; }
    [Required]
    public DateTime EndOffDayDate { get; set; }
    public DateTime? StartJobDate { get; set; }
    [Required]
    public string Reason { get; set; }
    [Required]
    public Guid Branch_Id { get; set; }
    [Required]
    public Guid Personal_Id { get; set; }
    [ForeignKey(nameof(Personal_Id))]
    public Personal Personal { get; set; }
}