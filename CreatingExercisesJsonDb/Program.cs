using System;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;

class Program
{
	static void Main()
	{
		CreateExerList.CreateJsonFile();

		GetExerList.GetDataFromJson();
	}
}

class GetExerList
{
	protected internal static void GetDataFromJson()
	{
		string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\CreatingExercisesJsonDb\exercises.json";
		string jsonContent = File.ReadAllText(directoryPath);

		using (JsonDocument document = JsonDocument.Parse(jsonContent))
		{
			// Pobieranie korzenia(całego obiektu) dokumentu
			JsonElement root = document.RootElement;

			// Iterowanie po właściwościach i wyświetlanie wartości
			foreach (JsonProperty property in root.EnumerateObject())
			{
				Console.WriteLine($"{property.Value}");

				Exercise? exer = JsonSerializer.Deserialize<Exercise>(property.Value);

			}
		}
	}
}

class CreateExerList
{
	protected internal static void CreateJsonFile()
	{
		string directoryPath = @"C:\Users\pawel\OneDrive\Pulpit\IT\Programowanie_nauka\c#\brudnopis\newApp\exercises";

		string[] dirs = Directory.GetDirectories(directoryPath);
		string jsonData = "{";
		int id = 0;

		foreach (string dir in dirs)
		{
			jsonData += ReadDataFromDirectory(dir, id) + ",";
			id++;
		}

		jsonData = jsonData.Substring(0, jsonData.Length - 1);
		jsonData += "}";

		File.WriteAllText(@"C:\Users\pawel\source\repos\YourTrainerApp2\CreatingExercisesJsonDb\exercises.json", jsonData);
		// Console.WriteLine(jsonData);
	}

	private static string ReadDataFromDirectory(string directoryPath, int id)
	{
		// Console.WriteLine(File.ReadAllText(directoryPath + @"\exercise.json"));
		string jsonString = File.ReadAllText(directoryPath + @"\exercise.json");

		Exercise? exer = JsonSerializer.Deserialize<Exercise>(jsonString);
		// Console.WriteLine(exer.name);

		string jsonPartToFile = $"\"{id}\" : {jsonString}";
		return jsonPartToFile;
	}
}

class Exercise
{
	public string? name { get; set; }
	public string? force { get; set; }
	public string? level { get; set; }
	public string? mechanic { get; set; }
	public string? equipment { get; set; }
	public string[]? primaryMuscles { get; set; }
	public string[]? secondaryMuscles { get; set; }
	public string[]? instructions { get; set; }
	public string? category { get; set; }

}