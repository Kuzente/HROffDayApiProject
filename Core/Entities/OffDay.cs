﻿using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Attributes;

namespace Core.Entities;
[EntityField(EntityName = "İzinler",IsShow = true,Sort = 4)]
public partial class OffDay : BaseEntity
{
	[Required, DisplayName("İzin Başlangıç Tarihi")]
	public DateTime StartDate { get; set; }
	[Required, DisplayName("İzin Bitiş Tarihi")]
	public DateTime EndDate { get; set; }
	[Required, DisplayName("Toplam İzin Günü")] 
	public int CountLeave { get; set; } 
	[DisplayName("Haftalık İzin")]
	public int LeaveByWeek { get; set; }
	[DisplayName("Yıllık İzin")]
	public int LeaveByYear { get; set; } 
	[DisplayName("Resmi İzin")]
	public int LeaveByPublicHoliday { get; set; } 
	[DisplayName("Ücretsiz İzin")]
	public int LeaveByFreeDay { get; set; } 
	[DisplayName("Alacak İzin")]
	public int LeaveByTaken { get; set; } 
	[DisplayName("Seyahat İzin")]
	public int LeaveByTravel { get; set; } 
	[DisplayName("Ölüm İzin")]
	public int LeaveByDead { get; set; } 
	[DisplayName("Babalık İzin")]
	public int LeaveByFather { get; set; }
	[DisplayName("Evlilik İzin")]
	public int LeaveByMarried { get; set; }
	[DisplayName("İzin Durumu")]
	public OffDayStatusEnum OffDayStatus { get; set; }
	[DisplayName("Açıklama")]
	public string? Description { get; set; }
	public Guid BranchId { get; set; }
	public Guid PositionId { get; set; }
	[DisplayName("Pdf Döküman Numarası")]
	public int DocumentNumber { get; set; }

	[DisplayName("Onaylayan İnsan Kaynakları Adı-Soyadı")]
	public string? HrName { get; set; }
	[DisplayName("Onaylayan Genel Müdür Adı-Soyadı")]
	public string? DirectorName { get; set; }
	[DisplayName("Onaylanan İzin Kullanılan Yıllık İzin Miktarı")]
	public int PdfUsedYearLeave { get; set; }
	[DisplayName("Onaylanan İzin Kalan Yıllık İzin Miktarı")]
	public int PdfRemainYearLeave { get; set; }
	[DisplayName("Onaylanan İzin Kalan Alacak İzin Miktarı")]
	public double PdfRemainTakenLeave { get; set; }

}
public partial class OffDay
{
	[Required]
	public Guid Personal_Id { get; set; }
	[ForeignKey(nameof(Personal_Id)),Required]
	public Personal Personal { get; set; }
}
