using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public partial class Personal : BaseEntity
{
	[DisplayName("Personal Adı - Soyadı"), Required]
	public string NameSurname { get; set; } 
	[DisplayName("Toplam Yıllık İzin"), Required]
	public int TotalYearLeave { get; set; } 
	[DisplayName("Doğum Tarihi"), Required]
	public DateTime BirthDate { get; set; }
	[DisplayName("İşe Başlama Tarihi"), Required]
	public DateTime StartJobDate { get; set; }
	[DisplayName("İşten Ayrılış Tarihi")]
	public DateTime? EndJobDate { get; set; } 
	[DisplayName("TC Kimlik No"), Required]
	public string IdentificationNumber { get; set; }
	[DisplayName("Sicil No"), Required]
	public string RegistirationNumber { get; set; }
	[DisplayName("Telefon No")]
	public string? Phonenumber { get; set; }
	[DisplayName("Özel Durum")]
	public bool RetiredOrOld { get; set; }
	[DisplayName("Emeklilik Tarihi")]
	public DateTime? RetiredDate { get; set; }
	[DisplayName("Cinsiyet")]
	public string Gender { get; set; }
	[DisplayName("Kullanılan Yıllık İzin"), Required]
	public int UsedYearLeave { get; set; } 
	[DisplayName("Toplam Alacak İzin"), Required]
	public int TotalTakenLeave { get; set; } 
	[DisplayName("Gıda Yardımı"), Required]
	public int FoodAid { get; set; } 
	[DisplayName("Gıda Yardımı Tarihi")] 
	public DateTime FoodAidDate { get; set; }

}
public partial class Personal
{
	[Required]
	public Guid Branch_Id { get; set; }
	[ForeignKey(nameof(Branch_Id))]
	public Branch Branch { get; set; }
	[Required]
	public Guid Position_Id { get; set; }
	[ForeignKey(nameof(Position_Id))]
	public Position Position { get; set; }
	
	public ICollection<OffDay> OffDays { get; set; }
	[Required]
	public Guid PersonalDetails_Id { get; set; }
	public PersonalDetails PersonalDetails { get; set; }
	
}