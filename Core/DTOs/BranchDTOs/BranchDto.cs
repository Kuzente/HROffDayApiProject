using Core.DTOs.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.DTOs.BranchDTOs;

public class BranchDto : BaseDto
{
    public string Name { get; set; }
    public int Count { get; set; }
}
