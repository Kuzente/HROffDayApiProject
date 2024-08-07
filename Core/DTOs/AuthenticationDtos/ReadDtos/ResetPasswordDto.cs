using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.AuthenticationDtos.ReadDtos
{
	public class ResetPasswordDto
	{
		[Required]
		public string UserId { get; set; }
		[Required, StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "Şifreniz en az 6,en fazla 20 karakter uzunluğunda olabilir!")]
		[RegularExpression(@"^(?=.*[a-zçğıöşü])(?=.*[A-ZÇĞİÖŞÜ])(?=.*\d).{6,20}$", ErrorMessage = "Şifreniz en az bir küçük harf,bir büyük harf ve bir rakam içermelidir!")]
		public string Password { get; set; }
		[Required]
		[Compare(nameof(Password),ErrorMessage ="Girdiğiniz şifreler eşleşmiyor!")]
		public string PasswordAgain { get; set; }
		[Required]
		public string Token { get; set; }
		[Required]
		public string Mail { get; set; }
		[Required]
		public string Security { get; set; }
	}
}
