using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Attributes;
using Core.Enums;

namespace Core.Entities;
[EntityField(EntityName = "Personeller",IsShow = true,Sort = 1)]
public partial class Personal : BaseEntity
{
	[DisplayName("Personal Adı - Soyadı"), Required]
	[PropertyField(PropertyName = "Adı-Soyadı",PropertyType = PropertyTypeEnum.Text,IsShow = true)]
	public string NameSurname { get; set; } 
	[DisplayName("Toplam Yıllık İzin"), Required]
	[PropertyField(PropertyName = "Hak Edilen Yıllık İzin Miktarı",PropertyType = PropertyTypeEnum.Number,IsShow = true)]
	public int TotalYearLeave { get; set; } 
	[DisplayName("Doğum Tarihi"), Required]
	[PropertyField(PropertyName = "Doğum Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime BirthDate { get; set; }
	[DisplayName("İşe Başlama Tarihi"), Required]
	[PropertyField(PropertyName = "İşe Başlama Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime StartJobDate { get; set; }
	[DisplayName("İşten Ayrılış Tarihi")]
	[PropertyField(PropertyName = "İşten Çıkış Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime? EndJobDate { get; set; } 
	[DisplayName("TC Kimlik No"), Required]
	[PropertyField(PropertyName = "Tc Kimlik Numarası",PropertyType = PropertyTypeEnum.Text,IsShow = true)]
	public string IdentificationNumber { get; set; }
	[DisplayName("Sicil No"), Required]
	[PropertyField(PropertyName = "Sicil Numarası",PropertyType = PropertyTypeEnum.Number,IsShow = true)]
	public int RegistirationNumber { get; set; }
	[DisplayName("Telefon No")]
	[PropertyField(PropertyName = "Telefon",PropertyType = PropertyTypeEnum.Text,IsShow = true)]
	public string? Phonenumber { get; set; }
	[DisplayName("Emeklilik Durumu")]
	[PropertyField(PropertyName = "Emeklilik Durumu",PropertyType = PropertyTypeEnum.Radio,IsShow = true)]
	public bool RetiredOrOld { get; set; }
	[DisplayName("Emeklilik Tarihi")]
	[PropertyField(PropertyName = "Emeklilik Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime? RetiredDate { get; set; }
	[DisplayName("Cinsiyet")]
	[PropertyField(PropertyName = "Cinsiyet",PropertyType = PropertyTypeEnum.List,IsShow = true)]
	public string Gender { get; set; }
	[DisplayName("Kullanılan Yıllık İzin"), Required]
	[PropertyField(PropertyName = "Kullanılan Yıllık İzin Miktarı",PropertyType = PropertyTypeEnum.Number,IsShow = true)]
	public int UsedYearLeave { get; set; } 
	[DisplayName("Toplam Alacak İzin"), Required]
	[PropertyField(PropertyName = "Mevcut Alacak İzin Miktarı",PropertyType = PropertyTypeEnum.Double,IsShow = true)]
	public double TotalTakenLeave { get; set; }
	[DisplayName("Gıda Yardımı"), Required]
	[PropertyField(PropertyName = "Gıda Yardımı Miktarı",PropertyType = PropertyTypeEnum.Number,IsShow = true)]
	public int FoodAid { get; set; } 
	[DisplayName("Gıda Yardımı Tarihi")] 
	[PropertyField(PropertyName = "Gıda Yardımı Yenilenme Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime FoodAidDate { get; set; }
	[DisplayName("Yıllık İzin Yenilenme Tarihi")] 
	[PropertyField(PropertyName = "Yıllık İzin Yenilenme Tarihi",PropertyType = PropertyTypeEnum.Date,IsShow = true)]
	public DateTime YearLeaveDate { get; set; }
	[DisplayName("Yıllık İzin Emeklilik Durumu")] 
	[PropertyField(PropertyName = "Yıllık İzini Emeklilik Durumu İle Yenilenenler",PropertyType = PropertyTypeEnum.Radio,IsShow = true)]
	public bool IsYearLeaveRetired { get; set; }
	[DisplayName("İşe Geri Alındı Mı Durumu")] 
	[PropertyField(PropertyName = "İşe Geri Alınma Durumu",PropertyType = PropertyTypeEnum.Radio,IsShow = true)]
	public bool IsBackToWork { get; set; }

}
public partial class Personal
{
	[Required]
	[PropertyField(PropertyName = "Şube Id",PropertyType = PropertyTypeEnum.Guid,IsShow = false)]
	public Guid Branch_Id { get; set; }
	[ForeignKey(nameof(Branch_Id))]
	[PropertyField(PropertyName = "Şube",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public Branch Branch { get; set; }
	[Required]
	[PropertyField(PropertyName = "Ünvan Id",PropertyType = PropertyTypeEnum.Guid,IsShow = false)]
	public Guid Position_Id { get; set; }
	[ForeignKey(nameof(Position_Id))]
	[PropertyField(PropertyName = "Ünvan",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public Position Position { get; set; }
	[PropertyField(PropertyName = "İzinler",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public ICollection<OffDay> OffDays { get; set; }
	[Required]
	[PropertyField(PropertyName = "Personel Detayları Id",PropertyType = PropertyTypeEnum.Guid,IsShow = false)]
	public Guid PersonalDetails_Id { get; set; }
	[PropertyField(PropertyName = "Personel Detayları",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public PersonalDetails PersonalDetails { get; set; }
	[PropertyField(PropertyName = "Görevlendirmeler",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public ICollection<TransferPersonal> TransferPersonals { get; set; }
	[PropertyField(PropertyName = "Eksik Günler",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public ICollection<MissingDay> MissingDays { get; set; }
	[PropertyField(PropertyName = "Personel Kümülatifleri",PropertyType = PropertyTypeEnum.Class,IsShow = false)]
	public ICollection<PersonalCumulative> PersonalCumulatives { get; set; }
	
}