using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;

class Program
{
	static void Main()
	{
		//CreateExerList.CreateJsonFile();

		//GetExerList.GetDataFromJson();

		MakeJpgList.DeleteJsonFiles();

		//MakeJpgList.RemoveJpgFromInternalFolder();
	}
}

class MakeJpgList
{
	protected internal static void DeleteJsonFiles()
	{
		string directoryPath = @"C:\Users\pawel\OneDrive\Pulpit\IT\exercises";
		string[] dirs = Directory.GetDirectories(directoryPath);

		foreach (string dir in dirs)
		{
			//File.Delete(dir + @"\exercise.json");
			
			Directory.Delete(dir + @"\images", true);

		}
	}

	protected internal static void RemoveJpgFromInternalFolder()
	{
		string directoryPath = @"C:\Users\pawel\OneDrive\Pulpit\IT\exercises";
		string[] dirs = Directory.GetDirectories(directoryPath);

		foreach(string dir in dirs)
		{
			File.Copy(dir + @"\images\0.jpg", Path.Combine(dir, "0.jpg"), true);
			File.Copy(dir + @"\images\1.jpg", Path.Combine(dir, "1.jpg"), true);

		}
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

			// Iterowanie po obiektach i wyświetlanie wartości
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
		string pathToJpg = "";

		Exercise? exer = JsonSerializer.Deserialize<Exercise>(jsonString);
		// Console.WriteLine(exer.name);

		string jsonPartToFile = $"\"{id}\" : {jsonString}";
		return jsonPartToFile;
	}
}

class Exercise
{
	[Key]
	public string Id { get; set; }
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