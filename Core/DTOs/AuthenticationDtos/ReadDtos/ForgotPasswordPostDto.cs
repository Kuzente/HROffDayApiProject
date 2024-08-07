using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.AuthenticationDtos.ReadDtos
{
	public class ForgotPasswordPostDto
	{
		[Required]
		public string Security { get; set; }
		[Required]
		public string Email { get; set; }
    }
}
