using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDtos.ReadDtos
{
	public class ReadUserDto
	{
		public Guid ID { get; set; }
		public string Username { get; set; }
		public UserRoleEnum Role { get; set; }
	}
}
