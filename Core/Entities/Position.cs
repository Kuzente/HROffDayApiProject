using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Attributes;

namespace Core.Entities;
[EntityField(EntityName = "Ünvanlar",IsShow = true,Sort = 3)]
public class Position : BaseEntity
{
	[DisplayName("Ünvan Adı"),Required]
	public string Name { get; set; }
	public ICollection<Personal> Personals { get; set; }
}