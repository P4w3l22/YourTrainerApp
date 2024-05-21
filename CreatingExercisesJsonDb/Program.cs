using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


class Program
{
	static void Main()
	{
		//CreateExerList.CreateJsonFile();

		GetExerList.GetDataFromJson();

		//JpgList.DeleteJsonFiles();

		//JpgList.RemoveJpgFromInternalFolder();

		//TestExerList.GetObjValues();

		//JpgList.GetJpgsPaths();

	}
}

class JpgList
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

	protected internal static void GetJpgsPaths()
	{
		string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\YourTrainerApp2\wwwroot\exercises_img\";
		string[] dirs = Directory.GetDirectories(directoryPath);
		foreach (string dir in dirs)
		{
			string name = Path.GetFileName(dir);
			Console.WriteLine(name);
		}
	}

}

class GetExerList
{
	protected internal static void GetDataFromJson()
	{
		string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\CreatingExercisesJsonDb\exercisesList.json";
		string jsonContent = File.ReadAllText(directoryPath);
		int id = 1;
		int counter = 0;

		string exercisesListTxt = string.Empty;

		using (JsonDocument document = JsonDocument.Parse(jsonContent))
		{
			// Pobieranie korzenia(całego obiektu) dokumentu
			JsonElement root = document.RootElement;

			// Iterowanie po obiektach i wyświetlanie wartości
			foreach (JsonProperty property in root.EnumerateObject())
			{
				//Console.WriteLine($"{property.Value}");

				dynamic exer = JsonConvert.DeserializeObject(property.Value.ToString());
				Exercise exercise = new();

				foreach (var item in exer)
				{
					switch (item.Name)
					{
						case "name":
							exercise.Name = item.Value.ToString().Replace("'", "''");
							break;
						case "force":
							exercise.Force = item.Value.ToString();
							break;
						case "level":
							exercise.Level = item.Value.ToString();
							break;
						case "mechanic":
							exercise.Mechanic = item.Value.ToString();
							break;
						case "equipment":
							exercise.Equipment = item.Value.ToString();
							break;
						case "primaryMuscles":
							var substring = item.Value.ToString().Substring(6, item.Value.ToString().Length - 10);
							exercise.PrimaryMuscles = substring;

							break;
						case "secondaryMuscles":
							var substringSecondaryMuscles = string.Empty;
							if (item.Value.ToString().Length > 4)
							{
								substringSecondaryMuscles = item.Value.ToString().Substring(6, item.Value.ToString().Length - 10);
								substringSecondaryMuscles = substringSecondaryMuscles.Replace("\",\r\n ", ";");
								substringSecondaryMuscles = substringSecondaryMuscles.Replace(" \"", "");
							}
							exercise.SecondaryMuscles = substringSecondaryMuscles;
							break;
						case "instructions":
							string instructions = item.Value.ToString();
							
							instructions = instructions.Substring(1, instructions.Length - 2);
							instructions = instructions.Replace("\",", ";");
							instructions = instructions.Replace("\"", "");
							instructions = instructions.Replace("\r\n ", "");
							instructions = instructions.Replace("\r\n", "");
							instructions = instructions.Replace("'", "''");
							
							exercise.Instructions = instructions;
							break;
						case "category":
							exercise.Category = item.Value.ToString();
							break;
					}

				}

				exercisesListTxt += $"('{exercise.Category}', " +
				                    $"'{exercise.Equipment}', " +
				                    $"'{exercise.Force}', " +
				                    $"'exercisesListImg\\{exercise.Name.Replace(" ", "_").Replace("'", "''")}\\0.jpg', " +
				                    $"'exercisesListImg\\{exercise.Name.Replace(" ", "_").Replace("'", "''")}\\1.jpg', " +
				                    $"'{exercise.Instructions}', " +
				                    $"'{exercise.Level}', " +
				                    $"'{exercise.Mechanic}', " +
				                    $"'{exercise.Name}', " +
				                    $"'{exercise.PrimaryMuscles}', " +
				                    $"'{exercise.SecondaryMuscles}'),\n";
				//Exercise exercise = new Exercise
				//{
				//	Id = id,
				//	Name = exer[0].Value,
				//	Force = exer[1].Value,
				//	Level = exer[2].Value,
				//	Mechanic = exer[3].Value,
				//	Equipment = exer[4].Value,
				//	PrimaryMuscles = exer[5].Value,
				//	SecondaryMuscles = exer[6].Value,
				//	Instructions = exer[7].Value,
				//	Category = exer[8].Value
				//	//ImgPath = exer[9]
				//};
				id++;
				Console.WriteLine(id);

				//Exercise? exer = JsonSerializer.Deserialize<Exercise>(property.Value);

			}
		}
		
		string exercisesListTxtPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\CreatingExercisesJsonDb\exercisesLists.txt";
				
		File.WriteAllText(exercisesListTxtPath, exercisesListTxt);

	}
}

class TestExerList
{
	protected internal static void GetObjValues()
	{
		//string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\CreatingExercisesJsonDb\exercises.json";
		//string jsonContent = File.ReadAllText(directoryPath);

		//using (JsonDocument document = JsonDocument.Parse(jsonContent))
		//{
		//	// Pobieranie korzenia(całego obiektu) dokumentu
		//	JsonElement root = document.RootElement;

		//	// Iterowanie po obiektach i wyświetlanie wartości
		//	foreach (JsonProperty property in root.EnumerateObject())
		//	{
		//		//Console.WriteLine($"{property.Value}");

		//		Exercise? exer = JsonSerializer.Deserialize<Exercise>(property.Value);
		//		Console.WriteLine(exer.name);
		//		Console.WriteLine(exer.force);
		//		Console.WriteLine(exer.level);
		//		Console.WriteLine(exer.mechanic);
		//		Console.WriteLine(exer.equipment);

		//	}
		//}

		string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\CreatingExercisesJsonDb\exercises.json";
		string jsonContent = File.ReadAllText(directoryPath);
		List<Exercise> exerList = new();
		int id = 1;

		JArray jsonArray = JArray.Parse(jsonContent);

		foreach (JObject jsonObject in jsonArray)
		{
			foreach (JProperty item in jsonObject.Properties())
			{
				string key = item.Name;
				JToken value = item.Value;

				Console.WriteLine($"Klucz: {key}, \nWartość: {value}");
			}
		}



	}

	//protected static void GetValues(string directoryPath)
	//{
	//	// Console.WriteLine(File.ReadAllText(directoryPath + @"\exercise.json"));
	//	string jsonString = File.ReadAllText(directoryPath);

	//	Exercise? exer = JsonSerializer.Deserialize<Exercise>(jsonString);
	//	Console.WriteLine(exer.name);
	//	Console.WriteLine(exer.force);
	//	Console.WriteLine(exer.level);
	//	Console.WriteLine(exer.mechanic);
	//	Console.WriteLine(exer.equipment);

	//}
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

		//Exercise? exer = Newtonsoft.Json.JsonSerializer.Deserialize<Exercise>(jsonString);
		// Console.WriteLine(exer.name);

		string jsonPartToFile = $"\"{id}\" : {jsonString}";
		return jsonPartToFile;
	}
}

class Exercise
{
	[Key]
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Force { get; set; }
	public string? Level { get; set; }
	public string? Mechanic { get; set; }
	public string? Equipment { get; set; }
	public string? PrimaryMuscles { get; set; }
	public string? SecondaryMuscles { get; set; }
	public string? Instructions { get; set; }
	public string? Category { get; set; }

}