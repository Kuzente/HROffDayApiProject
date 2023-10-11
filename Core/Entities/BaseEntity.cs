using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces.Base;

namespace Core.Entities
{
	public class BaseEntity : IBaseEntity
	{
		[Key, Required, DisplayName("ID")]
		public int ID { get; set; }
		[Required, DisplayName("Oluşturulma Tarihi")]
		public DateTime CreatedAt { get; set; }
		[Required, DisplayName("Düzenlenme Tarihi")]
		public DateTime ModifiedAt { get; set; }
		[Required, DisplayName("Silinme Tarihi")]
		public DateTime DeletedAt { get; set; }
		[Required, DefaultValue(EntityStatusEnum.Offline), DisplayName("Durum")]
		public EntityStatusEnum Status { get; set; }
       

    }
}
