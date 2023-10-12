using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Entities
{
	public class Branch : BaseEntity
	{
		[DisplayName("Şube Adı"), Required]
		public string Name { get; set; }
        public ICollection<Personal> Personals{ get; set; }
    }
}
