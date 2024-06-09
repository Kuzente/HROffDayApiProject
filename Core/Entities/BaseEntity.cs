using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Core.Attributes;
using Core.Enums;
using Core.Interfaces.Base;

namespace Core.Entities;

public class BaseEntity : IBaseEntity
{
	[Key, Required, DisplayName("ID")]
	[PropertyField(PropertyName = "ID",PropertyType = PropertyTypeEnum.Guid,IsShow = true)]
	public Guid ID { get; set; }
	[Required, DisplayName("Oluşturulma Tarihi")]
	[PropertyField(PropertyName = "Oluşturulma Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime CreatedAt { get; set; }
	[Required, DisplayName("Düzenlenme Tarihi")]
	public DateTime ModifiedAt { get; set; }
	[Required, DisplayName("Silinme Tarihi")]
	public DateTime DeletedAt { get; set; }
	[Required, DefaultValue(EntityStatusEnum.Offline), DisplayName("Durum")]
	public EntityStatusEnum Status { get; set; }
       

}