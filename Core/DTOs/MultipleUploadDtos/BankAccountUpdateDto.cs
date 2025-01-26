using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.MultipleUploadDtos;
public class BankAccountUpdateDto
{
	public Guid Id { get; set; }
	public string NameSurname { get; set; }
	public string? BankAccount { get; set; }
	public string? NewBankAccount { get; set; }
}
