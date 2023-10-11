using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Position : BaseEntity
	{
		[DisplayName("Ünvan Adı"),Required]
		public string Name { get; set; }
        public ICollection<Personal> Personals { get; set; }
    }
}
