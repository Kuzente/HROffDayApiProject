using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.MultipleUploadDtos;
public class SalaryUpdateDto
{
	public Guid Id { get; set; }
	public string NameSurname { get; set; }
	public double Salary { get; set; }
	public double NewSalary { get; set; }
}
