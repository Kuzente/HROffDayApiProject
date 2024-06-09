using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Core.Attributes;
using Core.Enums;

namespace Core.Entities;
[EntityField(EntityName = "Şubeler",IsShow = true,Sort = 2)]
public class Branch : BaseEntity
{
	[DisplayName("Şube Adı"), Required]
	[PropertyField(PropertyName = "Şube Adı",PropertyType = PropertyTypeEnum.List,IsShow = true)]
	public string Name { get; set; }
	[PropertyField(PropertyName = "Personeller",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public ICollection<Personal> Personals{ get; set; }
	public ICollection<BranchUser> BranchUsers { get; set; }
	
}