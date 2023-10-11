using Core.DTOs.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.PersonalDTOs
{
	public class WritePersonalDto : WriteBaseDto
	{
        public string NameSurname { get; set; }
		public DateTime BirthDate { get; set; }
		public string IdentificationNumber { get; set; }
		public string RegistirationNumber { get; set; }
		public int Branch_Id { get; set; }
		public int Position_Id { get; set; }
	}
}
