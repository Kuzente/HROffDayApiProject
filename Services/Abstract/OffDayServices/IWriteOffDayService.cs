﻿using Core.DTOs.OffDayDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.OffDayServices
{
	public interface IWriteOffDayService : IWriteService<ReadOffDayDto,WriteOffDayDto>
	{
	}
}
