using Core.DTOs.BaseDTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
	public class ResultWithDataDto<T> : IResultWithDataDto<T>
	{
		public bool IsSuccess { get; set; } = true;
		public string Err { get; set; } = "OK";
		public string Message { get; set; } = "OK";
		public T Data { get; set; }
		public IResultWithDataDto<T> SetData(T data)
		{
			Data = data;
			return this;
		}
		public IResultWithDataDto<T> SetErr(string err)
		{
			Err = err;
			return this;
		}
		public IResultWithDataDto<T> SetMessage(string message)
		{
			Message = message;
			return this;
		}
		public IResultWithDataDto<T> SetStatus(bool statusValue = true)
		{
			IsSuccess = statusValue;
			return this;
		}
	}
}
