using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Models.VM;

namespace YourTrainer_App.Areas.Visitor.Services;

public class TrainingPlanDataService : ITrainingPlanDataService
{
	private readonly ITrainingPlanService _trainingPlanService;
	private readonly ITrainingPlanExerciseService _trainingPlanExerciseService;
	private readonly IExerciseService _exerciseService;
	private readonly IAssignedTrainingPlanService _assignedTrainingPlanService;

	public TrainingPlanDataService(ITrainingPlanService trainingPlanService, ITrainingPlanExerciseService trainingPlanExerciseService, IExerciseService exerciseService, IAssignedTrainingPlanService assignedTrainingPlanService)
	{
		_trainingPlanService = trainingPlanService;
		_trainingPlanExerciseService = trainingPlanExerciseService;
		_exerciseService = exerciseService;
		_assignedTrainingPlanService = assignedTrainingPlanService;
	}

	public async Task SetTrainingPlanToClient(int trainerId, int clientId, int planId, string token)
	{
		var apiResponse = await _assignedTrainingPlanService.CreateAsync<APIResponse>(new AssignedTrainingPlan()
		{
			TrainerId = trainerId,
			ClientId = clientId,
			PlanId = planId
		}, token);
	}

	public async Task<List<TrainingPlan>> GetUserTrainingPlans(string sessionUsername)
	{
		var apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();

		bool isNotUsernameSet = sessionUsername is null || sessionUsername.Length == 0;

		return JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result))
						   .Where(tp => tp.Creator == (isNotUsernameSet ? "admin@gmail.com" : sessionUsername))
						   .ToList();
	}

	public List<int> GetPreviousTrainingPlanExercises(List<TrainingPlanExercise> trainingPlanExercises)
	{
		List<int> previousExercisesId = new();

		foreach (TrainingPlanExercise exercise in trainingPlanExercises)
		{
			previousExercisesId.Add(exercise.Id);
		}
		return previousExercisesId;
	}

	public async Task CreateTrainingPlan(TrainingPlan trainingPlan)
	{
		await _trainingPlanService.CreateAsync<APIResponse>(trainingPlan);

		await CreateTrainingPlanExercises(trainingPlan.Exercises, trainingPlan.Title, trainingPlan.Creator);
	}

	public async Task<int> GetTrainingPlanId(string title, string creator)
	{
		APIResponse apiResponse = await _trainingPlanService.GetAllAsync<APIResponse>();
		TrainingPlan trainingPlanDb = JsonConvert.DeserializeObject<List<TrainingPlan>>(Convert.ToString(apiResponse.Result))
						.Where(tp => tp.Title == title && tp.Creator == creator)
						.FirstOrDefault();
		return trainingPlanDb.Id;
	}

	// ujednolicić insert z create
	private async Task CreateTrainingPlanExercises(List<TrainingPlanExercise> trainingPlanExercises, string title, string creator)
	{
		int id = await GetTrainingPlanId(title, creator);

		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlanExercises)
		{
			await _trainingPlanExerciseService.InsertAsync<APIResponse>(new TrainingPlanExerciseCreateDTO()
			{
				TPId = id,
				EId = trainingPlanExercise.EId,
				Series = trainingPlanExercise.Series,
				Reps = trainingPlanExercise.Reps,
				Weights = trainingPlanExercise.Weights
			});
		}
	}

	public async Task UpdateTrainingPlan(TrainingPlan trainingPlan, List<int> previousExercusesId)
	{
		await _trainingPlanService.UpdateAsync<APIResponse>(trainingPlan);

		await UpdateTrainingPlanExercises(trainingPlan.Exercises, previousExercusesId, trainingPlan.Id);
	}

	private async Task UpdateTrainingPlanExercises(List<TrainingPlanExercise> trainingPlanExercises, List<int> previousExercisesId, int id)
	{
		// DODAĆ USUWANIE POPRZEDNICH REKORDÓW W BAZIE DANYCH
		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlanExercises)
		{
			
			if (trainingPlanExercise.Id == 0)
			{
				await _trainingPlanExerciseService.InsertAsync<APIResponse>(new TrainingPlanExerciseCreateDTO()
				{
					TPId = id,
					EId = trainingPlanExercise.EId,
					Series = trainingPlanExercise.Series,
					Reps = trainingPlanExercise.Reps,
					Weights = trainingPlanExercise.Weights
				});
			}
			else
			{
				previousExercisesId.Remove(trainingPlanExercise.Id);

				await _trainingPlanExerciseService.UpdateAsync<APIResponse>(new TrainingPlanExerciseUpdateDTO()
				{
					Id = trainingPlanExercise.Id,
					TPId = id,
					EId = trainingPlanExercise.EId,
					Series = trainingPlanExercise.Series,
					Reps = trainingPlanExercise.Reps,
					Weights = trainingPlanExercise.Weights
				});
			}
		}

		if (previousExercisesId.Count > 0)
		{
			foreach (int exerciseId in previousExercisesId)
			{
				await _trainingPlanExerciseService.DeleteAsync<APIResponse>(exerciseId);
			}
		}
	}

	public async Task<TrainingPlan> GetTrainingPlan(int id)
	{
		APIResponse apiResponse = await _trainingPlanService.GetAsync<APIResponse>(id);
		TrainingPlan trainingPlan = JsonConvert.DeserializeObject<TrainingPlan>(Convert.ToString(apiResponse.Result));
		trainingPlan.CreateTrainingDaysDict();

		return trainingPlan;
	}

	public async Task<List<TrainingPlanExerciseCreateVM>> GetTrainingPlanExercises(List<TrainingPlanExercise> trainingPlanExercises)
	{
		List<TrainingPlanExerciseCreateVM> exercises = new();
		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlanExercises)
		{
			exercises.Add(await GetExercise(trainingPlanExercise.EId));
		}

		return exercises;
	}

	private async Task<TrainingPlanExerciseCreateVM> GetExercise(int exerciseId)
	{
		APIResponse? apiResponse = await _exerciseService.GetAsync<APIResponse>(exerciseId);
		return JsonConvert.DeserializeObject<TrainingPlanExerciseCreateVM>(Convert.ToString(apiResponse.Result));
	}

	public async Task DeleteTrainingPlan(int id)
	{
		var apiResponse = await _trainingPlanService.DeleteAsync<APIResponse>(id);
		int i = 1;
	}

	public TrainingPlan IncrementExerciseSeriesAndGetTrainingPlan(TrainingPlan trainingPlan, int id)
	{
		TrainingPlan trainingPlanResult = trainingPlan;

		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
		{
			if (trainingPlanExercise.EId == id)
			{
				trainingPlanExercise.Series++;
				trainingPlanExercise.Reps += ";4";
				trainingPlanExercise.Weights += ";80";

				break;
			}
		}

		return trainingPlanResult;
	}

	public TrainingPlan DecrementExerciseSeriesAndGetTrainingPlan(TrainingPlan trainingPlan, int id)
	{
		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
		{
			if (trainingPlanExercise.EId == id)
			{
				if (trainingPlanExercise.Series > 0)
				{
					trainingPlanExercise.Series--;

					string[] reps = trainingPlanExercise.Reps.Split(";");
					string[] weights = trainingPlanExercise.Weights.Split(";");

					trainingPlanExercise.Reps = "";
					trainingPlanExercise.Weights = "";

					for (int i = 0; i < trainingPlanExercise.Series; i++)
					{
						trainingPlanExercise.Reps += reps[i] + ';';
						trainingPlanExercise.Weights += weights[i] + ';';
					}

					if (trainingPlanExercise.Weights.Length > 0)
					{
						trainingPlanExercise.Reps = trainingPlanExercise.Reps.Substring(0, trainingPlanExercise.Reps.Length - 1);
						trainingPlanExercise.Weights = trainingPlanExercise.Weights.Substring(0, trainingPlanExercise.Weights.Length - 1);
					}
				}

				break;
			}
		}

		return trainingPlan;
	}

	public async Task<TrainingPlan> DeleteExerciseAndGetTrainingPlan(TrainingPlan trainingPlan, int listPosition)
	{
		var id = trainingPlan.Exercises[listPosition].Id;

		//await _trainingPlanExerciseService.DeleteAsync<APIResponse>(id);

		trainingPlan.Exercises.RemoveAt(listPosition);

		return trainingPlan;
	}

	public List<TrainingPlanExerciseCreateVM> DeleteExerciseAndGetExercisesList(List<TrainingPlanExerciseCreateVM> exercises, int listPosition)
	{
		exercises.RemoveAt(listPosition);
		return exercises;
	}

	public TrainingPlan SaveRepsWeightsAndGetTrainingPlan(TrainingPlan trainingPlan, string values, string exerciseId, string seriesPosition)
	{
		int id = int.Parse(exerciseId);
		int seriesId = int.Parse(seriesPosition);
		string[] repsAndWeights = values.Split(';');

		foreach (TrainingPlanExercise trainingPlanExercise in trainingPlan.Exercises)
		{
			if (trainingPlanExercise.EId == id)
			{
				string[] reps = trainingPlanExercise.Reps.Split(";");
				string[] weights = trainingPlanExercise.Weights.Split(";");

				trainingPlanExercise.Reps = "";
				trainingPlanExercise.Weights = "";

				reps[seriesId] = repsAndWeights[0].ToString();
				weights[seriesId] = repsAndWeights[1].ToString();

				for (int i = 0; i < trainingPlanExercise.Series; i++)
				{
					trainingPlanExercise.Reps += reps[i] + ';';
					trainingPlanExercise.Weights += weights[i] + ';';
				}

				if (trainingPlanExercise.Weights.Length > 0)
				{
					trainingPlanExercise.Reps = trainingPlanExercise.Reps.Substring(0, trainingPlanExercise.Reps.Length - 1);
					trainingPlanExercise.Weights = trainingPlanExercise.Weights.Substring(0, trainingPlanExercise.Weights.Length - 1);
				}

				break;
			}
		}

		return trainingPlan;
	}

	public async Task<TrainingPlan> AddExerciseAndGetTrainingPlan(TrainingPlan trainingPlan, int id)
	{
		var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);
		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			TrainingPlanExerciseCreateVM exercise = JsonConvert.DeserializeObject<TrainingPlanExerciseCreateVM>(Convert.ToString(apiResponse.Result));

			if (trainingPlan.Exercises is null)
			{
				trainingPlan.Exercises = new();
			}

			trainingPlan.Exercises.Add(new()
			{
				EId = exercise.Id,
				Series = 1,
				Reps = "4",
				Weights = "80"
			});
		}

		return trainingPlan;
	}

	public async Task<List<TrainingPlanExerciseCreateVM>> AddExerciseAndGetExercisesList(List<TrainingPlanExerciseCreateVM> exercises, int id)
	{
		if (exercises is null)
		{
			exercises = new();
		}

		var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);
		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			TrainingPlanExerciseCreateVM? exercise = JsonConvert.DeserializeObject<TrainingPlanExerciseCreateVM>(Convert.ToString(apiResponse.Result));
			if (exercise is not null)
			{
				exercises.Add(exercise);
			}
		}

		return exercises;
	}

	public TrainingPlan SaveTitleAndGetTrainingPlan(TrainingPlan trainingPlan, string title)
	{
		trainingPlan.Title = title;
		return trainingPlan;
	}

	public TrainingPlan SaveTrainingDaysAndGetTrainingPlan(TrainingPlan trainingPlan, string day)
	{
		if (trainingPlan.TrainingDaysDict[day])
		{
			trainingPlan.TrainingDaysDict[day] = false;
		}
		else
		{
			trainingPlan.TrainingDaysDict[day] = true;
		}

		return trainingPlan;
	}

	public TrainingPlan SaveNotesAndGetTrainingPlan(TrainingPlan trainingPlan, string notes)
	{
		trainingPlan.Notes = notes;
		return trainingPlan;
	}
}
