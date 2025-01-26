using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.MultipleUploadDtos;
public class IbanUpdateDto
{
	public Guid Id { get; set; }
	public string NameSurname { get; set; }
	public string? IBAN { get; set; }
	public string? NewIBAN { get; set; }
}
