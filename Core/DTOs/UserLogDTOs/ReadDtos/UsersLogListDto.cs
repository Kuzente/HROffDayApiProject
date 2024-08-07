using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserLogDTOs.ReadDtos
{
	public class UsersLogListDto
	{
		public DateTime CreatedAt { get; set; }
		public LogType LogType { get; set; }
		public string Description { get; set; }
        public string Username { get; set; }
    }
}
