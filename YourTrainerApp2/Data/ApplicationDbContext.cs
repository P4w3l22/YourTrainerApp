using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourTrainerApp2.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace YourTrainerApp2.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Exercise> Exercises { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\YourTrainerApp2\exercises.json";
			string jsonContent = File.ReadAllText(directoryPath);
			List<Exercise> exerList = new();
			List<string> jpgList0 = GetImgUrlLink("0");
			List<string> jpgList1 = GetImgUrlLink("1");

			int id = 1;

			using (JsonDocument document = JsonDocument.Parse(jsonContent))
			{
				// Pobieranie korzenia(całego obiektu) dokumentu
				JsonElement root = document.RootElement;

				// Iterowanie po obiektach i wyświetlanie wartości
				foreach (JsonProperty property in root.EnumerateObject())
				{
					//Exercise exer = JsonSerializer.Deserialize<Exercise>(property.Value.ToString());
					dynamic exer = JsonConvert.DeserializeObject(property.Value.ToString());
					Exercise exercise = new();
					exercise.Id = id;
					exercise.ImgPath1 = jpgList0[id - 1];
					exercise.ImgPath2 = jpgList1[id - 1];


					foreach (var item in exer)
					{
						switch (item.Name)
						{
							case "name":
								exercise.Name = item.Value.ToString();
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
								exercise.PrimaryMuscles = ValidateList(item.Value.ToString());
								break;
							case "secondaryMuscles":
								exercise.SecondaryMuscles = ValidateList(item.Value.ToString());
								break;
							case "instructions":
								exercise.Instructions = ValidateList(item.Value.ToString());
								break;
							case "category":
								exercise.Category = item.Value.ToString();
								break;
							default:
								break;
						}

					}
					id++;

					exerList.Add(exercise);

				}
			};

			modelBuilder.Entity<Exercise>().HasData(exerList);
		}

		private string ValidateList(string value)
		{
			string output = string.Empty;

			if (value.Length > 2)
			{
				string subString = value.Replace("\r\n", "").Replace(",  ", ";").Replace("\"", "");
				output = subString.Substring(3, subString.Length - 4);
			}

			return output;
		}

		private List<string> GetImgUrlLink(string pointer)
		{
			string directoryPath = @"C:\Users\pawel\source\repos\YourTrainerApp2\YourTrainerApp2\wwwroot\exercises_img\";
			string[] dirs = Directory.GetDirectories(directoryPath);
			List<string> result = new();

			foreach (string dir in dirs)
			{
				result.Add(@"exercises_img\" + Path.GetFileName(dir) + @"\" + pointer + ".jpg");
			}

			return result;
		}
	}

}
