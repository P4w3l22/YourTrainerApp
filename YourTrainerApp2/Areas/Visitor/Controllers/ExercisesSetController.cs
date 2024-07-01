using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;
using YourTrainerApp.Models;

namespace YourTrainerApp.Areas.Visistor.Controllers;

[Area("Visitor")]
public class ExercisesSetController : Controller
{
	private readonly IExerciseService _exerciseService;

	public ExercisesSetController(IExerciseService exerciseService)
    {
		_exerciseService = exerciseService;
    }

    public IActionResult Index()
	{
		return View();
	}

	public async Task<IActionResult> Exercise(int id)
	{
		if (id == 0)
		{
			return NotFound();
		}
		var response = await _exerciseService.GetAsync<APIResponse>(id);
		Exercise exercise = JsonConvert.DeserializeObject<Exercise>(Convert.ToString(response.Result));

		if (exercise is null)
		{
			return NotFound();
		}

		if (exercise.ImgPath1 is not null && exercise.ImgPath2 is not null)
        {
			if (exercise.ImgPath1.Contains("''"))
			{
				exercise.ImgPath1 = exercise.ImgPath1.Replace("''", "'");
				exercise.ImgPath2 = exercise.ImgPath2.Replace("''", "'");
			}
		}
			

		return View(exercise);
	}

	public async Task<ActionResult> GetDynamicContent(string exerciseType) =>
        await GetSelectedExercisesData(exerciseType);

    private async Task<ActionResult> GetSelectedExercisesData(string exerciseType)
    {
        var selectedExercisesData = new List<string>();

        var primaryMusclesList = GetPrimaryMusclesList(exerciseType);
        foreach (string primaryMuscle in primaryMusclesList)
        {
            var apiResponse = await _exerciseService.GetAllAsync<APIResponse>();
            var filteredExercises = FilterExercisesByPrimaryMuscle(apiResponse.Result, primaryMuscle);
            foreach (var exercise in filteredExercises)
            {
                selectedExercisesData.Add(GetExerciseData(exercise));
            }
        }

        var content = string.Join(",", selectedExercisesData);

        return Content(content);
    }

	private List<string> GetPrimaryMusclesList(string exerciseType)
	{
        List<string> primaryMuscles = new();

        switch (exerciseType)
        {
            case "chest":
                primaryMuscles.Add("chest");
                break;
            case "back":
                primaryMuscles.Add("lats");
                primaryMuscles.Add("lower back");
                primaryMuscles.Add("middle back");
                primaryMuscles.Add("traps");
                primaryMuscles.Add("neck");
                break;
            case "shoulders":
                primaryMuscles.Add("shoulders");
                break;
            case "triceps":
                primaryMuscles.Add("triceps");
                break;
            case "biceps":
                primaryMuscles.Add("biceps");
                break;
            case "forearms":
                primaryMuscles.Add("forearms");
                break;
            case "legs":
                primaryMuscles.Add("abductor");
                primaryMuscles.Add("adductor");
                primaryMuscles.Add("glutes");
                primaryMuscles.Add("hamstrings");
                primaryMuscles.Add("quadriceps");
                break;
            case "abdominals":
                primaryMuscles.Add("abdominals");
                break;
            case "calves":
                primaryMuscles.Add("calves");
                break;
            default:
                break;
        }

		return primaryMuscles;
    }
	
    private List<Exercise> FilterExercisesByPrimaryMuscle(object exercises, string primaryMuscle) =>
        JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(exercises))
                    .Where(u => u.PrimaryMuscles == primaryMuscle)
                    .ToList();

    private string GetExerciseData(Exercise exercise)
    {
        if (exercise.ImgPath1 is null || exercise.ImgPath2 is null)
        {
            return exercise.Name + "&_&" + exercise.Id;
		}
        if (exercise.ImgPath1.Contains("''"))
        {
            exercise.ImgPath1 = exercise.ImgPath1.Replace("''", "'");
            exercise.ImgPath2 = exercise.ImgPath2.Replace("''", "'");
        }

        return exercise.Name + '&' + exercise.ImgPath1 + '&' + exercise.Id;
    }
}

