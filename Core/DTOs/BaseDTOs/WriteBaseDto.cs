using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.BaseDTOs
{
	public class WriteBaseDto : BaseDto
	{
		public int ID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public DateTime DeletedAt { get; set; }
		public EntityStatusEnum Status { get; set; }
	}
}
