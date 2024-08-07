using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.AuthenticationDtos.ReadDtos
{
    public class ConfirmForgotPassDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string mail { get; set; }
    }
}
