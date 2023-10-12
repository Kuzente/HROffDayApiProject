using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs
{
	public class ReadPersonalDto : ReadBaseDto
	{
		
		public string NameSurname { get; set; }
		
		public int TotalLeave { get; set; } 
		
		public DateTime BirthDate { get; set; }

		public string IdentificationNumber { get; set; } 
		
		public string RegistirationNumber { get; set; } 
		public int Branch_Id { get; set; }
        public ReadBranchDto Branch { get; set; }
        public ReadPositionDto Position { get; set; }


    }
}
