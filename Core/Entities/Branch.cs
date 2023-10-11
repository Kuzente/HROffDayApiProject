using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Branch : BaseEntity
	{
		[DisplayName("Şube Adı"), Required]
		public string Name { get; set; }
        public ICollection<Personal> Personals{ get; set; }
    }
}
