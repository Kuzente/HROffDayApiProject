﻿using Core.Interfaces;

namespace Core.DTOs;

public class ResultWithDataDto<T> : IResultWithDataDto<T>
{
	public bool IsSuccess { get; set; } = true;
	public string Err { get; set; } = "OK";
	public string Message { get; set; } = "OK";
	public T Data { get; set; }
	public int TotalRecords { get; set; } 
	public int TotalPages { get; set; }
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
	public IResultWithDataDto<T> SetTotalRecords(int totalRecords = 0)
	{
		TotalRecords = totalRecords;
		return this;
	}

	public IResultWithDataDto<T> SetTotalPages(int totalPages = 0)
	{
		TotalPages = totalPages;
		return this;
	}
}