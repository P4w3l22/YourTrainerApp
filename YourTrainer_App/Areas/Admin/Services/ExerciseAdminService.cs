using Newtonsoft.Json;
using System.Net;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainer_App.Areas.Admin.Controllers;
using YourTrainer_App.Models;
using YourTrainer_App.Models.VM;

namespace YourTrainer_App.Areas.Admin.Services;

public class ExerciseAdminService : IExerciseAdminService
{
	private readonly ILogger<ExerciseAdminController> _logger;
	private readonly IExerciseService _exerciseService;

	public ExerciseAdminService(IExerciseService exerciseService, ILogger<ExerciseAdminController> logger)
	{
		_exerciseService = exerciseService;
		_logger = logger;
	}

	public async Task<List<Exercise>> GetExercisesList()
	{
		List<Exercise> exerciseList = new();

		var apiResponse = await _exerciseService.GetAllAsync<APIResponse>();

		if (apiResponse is not null && apiResponse.IsSuccess)
		{
			exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(Convert.ToString(apiResponse.Result));
		}

		return exerciseList;
	}

	public async Task<(string, string)> CreateExerciseAndGetResponse(ExerciseCreateVM exerciseCreated, string sessionToken)
	{
		exerciseCreated.Exercise.Instructions = exerciseCreated.Exercise.Instructions.Replace("\r\n", "; ");

		var apiResponse = await _exerciseService.CreateAsync<APIResponse>(exerciseCreated.Exercise, sessionToken);

		if (apiResponse.StatusCode == HttpStatusCode.InternalServerError)
		{
			_logger.LogError(apiResponse.Errors.FirstOrDefault(), "Błąd podczas zapisu do bazy danych");
			return ("Error", "Wystąpił błąd podczas przetwarzania Twojego żądania. Spróbuj ponownie później.");
		}
		else if (apiResponse.StatusCode == HttpStatusCode.Created)
		{
			return ("Success", "Utworzono ćwiczenie");
		}
		else
		{
			return ("Error", apiResponse.Errors.FirstOrDefault());
		}
	}

	public async Task<(string, string)> UpdateExerciseAndGetResponse(ExerciseCreateVM exerciseUpdated, string sessionToken)
	{
		exerciseUpdated.Exercise.Instructions = exerciseUpdated.Exercise.Instructions.Replace("\r\n", "; ");
		var apiResponse = await _exerciseService.UpdateAsync<APIResponse>(exerciseUpdated.Exercise, sessionToken);

		if (apiResponse.StatusCode == HttpStatusCode.InternalServerError)
		{
			_logger.LogError(apiResponse.Errors.FirstOrDefault(), "Błąd podczas zapisu do bazy danych");
			return ("Error", "Wystąpił błąd podczas przetwarzania Twojego żądania. Spróbuj ponownie później.");
		}
		else if (apiResponse.StatusCode == HttpStatusCode.NoContent)
		{
			return ("Success", "Zaktualizowano ćwiczenie");
		}
		else
		{
			return ("Error", apiResponse.Errors.FirstOrDefault());
		}
	}

	public async Task<(string, string)> DeleteExerciseAndGetResponse(int id, string sessionToken)
	{
		APIResponse apiResponse = await _exerciseService.DeleteAsync<APIResponse>(id, sessionToken);

		if (apiResponse.StatusCode == HttpStatusCode.InternalServerError)
		{
			_logger.LogError(apiResponse.Errors.FirstOrDefault(), "Błąd podczas zapisu do bazy danych");
			return ("Error", "Wystąpił błąd podczas przetwarzania Twojego żądania. Spróbuj ponownie później.");
		}
		else if (apiResponse.StatusCode == HttpStatusCode.NoContent)
		{
			return ("Success", "Usunięto ćwiczenie");
		}
		else
		{
			return ("Error", apiResponse.Errors.FirstOrDefault());
		}
	}

	public async Task<Exercise> GetExercise(int id)
	{
		var apiResponse = await _exerciseService.GetAsync<APIResponse>(id);
		return JsonConvert.DeserializeObject<Exercise>(Convert.ToString(apiResponse.Result));
	}
}
