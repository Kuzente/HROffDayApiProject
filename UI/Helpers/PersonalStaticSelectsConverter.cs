namespace UI.Helpers
{
	public static class PersonalStaticSelectsConverter
	{
		public static string JsonToString()
		{
			string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "json", "personal-selects-data.json");
			string jsonContent = File.ReadAllText(jsonFilePath);
			return jsonContent;
		}
	}
}
