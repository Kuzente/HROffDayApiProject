using Core.DTOs.BaseDTOs;

namespace Services.Abstract;

public interface IService<T> where T : BaseDto
{
}