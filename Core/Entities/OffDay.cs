using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
	public partial class OffDay : BaseEntity
	{
		[Required, DisplayName("İzin Başlangıç Tarihi")]
		public DateTime StartDate { get; set; }
		[Required, DisplayName("İzin Bitiş Tarihi")]
		public DateTime EndDate { get; set; }
		[Required, DisplayName("Toplam İzin Günü")] 
		public int CountLeave { get; set; } = 0;
		[DisplayName("Haftalık İzin")]
		public int LeaveByWeek { get; set; } = 0;
		[DisplayName("Yıllık İzin")]
		public int LeaveByYear { get; set; } = 0;
		[DisplayName("Resmi İzin")]
		public int LeaveByPublicHoliday { get; set; } = 0;
		[DisplayName("Ücretsiz İzin")]
		public int LeaveByFreeDay { get; set; } = 0;
		[DisplayName("Alacak İzin")]
		public int LeaveByTaken { get; set; } = 0;
		[DisplayName("Seyahat İzin")]
		public int LeaveByTravel { get; set; } = 0;
		[DisplayName("Ölüm İzin")]
		public int LeaveByDead { get; set; } = 0;
		[DisplayName("Babalık İzin")]
		public int LeaveByFather { get; set; } = 0;
		[DisplayName("Evlilik İzin")]
		public int LeaveByMarried { get; set; } = 0;
		[DisplayName("İzin Durumu")]
		public OffDayStatusEnum OffDayStatus { get; set; }

	}
	public partial class OffDay
	{
		[Required]
		public int Personal_Id { get; set; }
		[ForeignKey(nameof(Personal_Id))]
		public Personal Personal { get; set; }
    }
}
