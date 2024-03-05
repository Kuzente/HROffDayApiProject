using Core.DTOs.BaseDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.PositionDTOs
{
    public class PositionDto : BaseDto
    {
        [Required]
        public string Name { get; set; }
    }
}
