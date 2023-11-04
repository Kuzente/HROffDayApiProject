using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.PersonalDTOs
{
    public class AddPersonalDto : BaseDto
    {
        public string NameSurname { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartJobDate { get; set; }
        public string IdentificationNumber { get; set; }
        public string RegistirationNumber { get; set; }
        public string Phonenumber { get; set; }
        public bool RetiredOrOld { get; set; }
        
        public string Gender { get; set; }
        public int TotalYearLeave { get; set; }
        public int Branch_Id { get; set; }
        public int Position_Id { get; set; }
       
    }
}
