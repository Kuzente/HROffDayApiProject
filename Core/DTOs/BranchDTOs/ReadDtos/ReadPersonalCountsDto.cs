using Core.Enums;


namespace Core.DTOs.BranchDTOs.ReadDtos
{
    public class ReadPersonalCountsDto
    {
        public string BranchName { get; set; }
		public List<DepartmentCountDto> DepartmentCounts { get; set; }
	}
	public class DepartmentCountDto
	{
		public string DepartmentName { get; set; }
		public int Count { get; set; }
	}
}
