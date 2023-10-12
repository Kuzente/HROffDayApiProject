using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
	public class Position : BaseEntity
	{
		[DisplayName("Ünvan Adı"),Required]
		public string Name { get; set; }
        public ICollection<Personal> Personals { get; set; }
    }
}
