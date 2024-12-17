namespace UI.Models
{
	public class ODataQueryParamsModel
	{
		public string? Filter { get; set; }
		public string? OrderBy { get; set; }
		public string? Expand { get; set; }
		public string Select { get; set; }
	}
}
