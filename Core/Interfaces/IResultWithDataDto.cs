using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IResultWithDataDto<T> 
	{
		public T Data { get; set; }
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public string Err { get; set; }
		public IResultWithDataDto<T> SetData(T data);
		public IResultWithDataDto<T> SetMessage(string message = "OK");
		public IResultWithDataDto<T> SetErr(string err = "OK");
		public IResultWithDataDto<T> SetStatus(bool isSuccess = true);
	}
}
