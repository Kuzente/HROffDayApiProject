using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Querys
{
	public class UserLogQuery
	{
		public string search { get; set; }
		public int sayfa { get; set; } = 1;
		public string sortName { get; set; }
		public string sortBy { get; set; }
		public LogType? LogType{ get; set; }
		public int? filterYear { get; set; }
		public int? filterMonth { get; set; }
	}
}
