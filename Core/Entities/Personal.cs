using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
	public partial class Personal : BaseEntity
	{
		[DisplayName("Personal Adı - Soyadı"), Required]
		public string NameSurname { get; set; }
		[DisplayName("Toplam İzin"), Required]
		public int TotalLeave { get; set; } = 0;
		[DisplayName("Doğum Tarihi"), Required]
		public DateTime BirthDate { get; set; }
		[DisplayName("TC Kimlik No"), Required]
		public string IdentificationNumber { get; set; }
		[DisplayName("Sicil No"), Required]
		public string RegistirationNumber { get; set; }


    }
	public partial class Personal
	{
		[Required]
        public int Branch_Id { get; set; }
		[ForeignKey(nameof(Branch_Id))]
        public Branch Branch { get; set; }
		[Required]
		public int Position_Id { get; set; }
		[ForeignKey(nameof(Position_Id))]
		public Position Position { get; set; }
		public ICollection<OffDay> OffDays { get; set; }
	}
}
