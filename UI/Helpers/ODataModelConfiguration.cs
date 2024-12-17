using Core.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace UI.Helpers
{
	public static class ODataModelConfiguration
	{
		public static IEdmModel GetEdmModel()
		{
			var modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<Personal>("Personal");
			return modelBuilder.GetEdmModel();
		}
	}
}
