using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainerApp.Models.DTO;
using YourTrainerApp.Models;
using YourTrainerApp.Models.VM;
using YourTrainerApp.Attributes;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Areas.Visitor.Services;
using Microsoft.AspNetCore.Http;

namespace YourTrainerApp.Areas.Visistor.Controllers;

[Area("Visitor")]
public class TrainingPlanController : Controller
{
    private readonly ITrainingPlanDataService _trainingPlanDataService;
    private string? _sessionUsername 
    {
        get => HttpContext.Session.GetString("Username");
        set => HttpContext.Session.SetString("Username", value); 
    }
    private TrainingPlan? _sessionTrainingPlan
    {
        get => JsonConvert.DeserializeObject<TrainingPlan>(HttpContext.Session.GetString("TrainingPlanData") ?? JsonConvert.SerializeObject(new TrainingPlan()));
        set => HttpContext.Session.SetString("TrainingPlanData", JsonConvert.SerializeObject(value));
    }
    private List<TrainingPlanExerciseCreateVM>? _sessionExercises
	{
		get => JsonConvert.DeserializeObject<List<TrainingPlanExerciseCreateVM>>(HttpContext.Session.GetString("Exercises") ?? JsonConvert.SerializeObject(new List<TrainingPlanExerciseCreateVM>()));
		set => HttpContext.Session.SetString("Exercises", JsonConvert.SerializeObject(value));
	}
    private List<int>? _sessionPreviousExercises
    {
        get => JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("PreviousExercises") ?? JsonConvert.SerializeObject(new List<int>()));
        set => HttpContext.Session.SetString("PreviousExercises", JsonConvert.SerializeObject(value));
	} 
    private string[]? _sessionTrainerClientId => (HttpContext.Session.GetString("SenderReceiverId") ?? "0;0").Split(";");


	public TrainingPlanController(ITrainingPlanDataService trainingPlanDataService)
	{
        _trainingPlanDataService = trainingPlanDataService;
	}


    public async Task<IActionResult> Index()
    {
        List<TrainingPlan> trainingPlans = await _trainingPlanDataService.GetUserTrainingPlans(_sessionUsername);
        return View(trainingPlans);
    }


    //[Authorize(Roles = "admin")]
    [HttpGet]
    //[AdminSessionCheck]
    public IActionResult Upsert(bool isEditing = true)
    {
        if (!isEditing)
        {
			_sessionExercises = new();
            _sessionTrainingPlan = new();
            _sessionPreviousExercises = new();
		}
		if (!_sessionTrainingPlan.Exercises.IsNullOrEmpty() && _sessionTrainingPlan.Exercises.Count() > 0 && _sessionPreviousExercises.IsNullOrEmpty())
		{
		    _sessionPreviousExercises = _trainingPlanDataService.GetPreviousTrainingPlanExercises(_sessionTrainingPlan.Exercises);
		}

        TrainingPlan trainingPlan = new();
        trainingPlan.Creator = _sessionUsername;

		if (_sessionTrainingPlan is null)
		{
			_sessionTrainingPlan = trainingPlan;
		}
		else
		{
            trainingPlan = _sessionTrainingPlan;
		}

		return View(trainingPlan);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [ClearSessionStrings]
    public async Task<IActionResult> Upsert(TrainingPlan tp)
    {
        TrainingPlan trainingPlan = _sessionTrainingPlan;
        trainingPlan.CreateTrainingDaysString();

        if (trainingPlan.Creator is null)
        {
            trainingPlan.Creator = _sessionUsername;
        }

        if (trainingPlan.Exercises is null)
        {
            trainingPlan.Exercises = new();
        }

        if (trainingPlan.Id == 0)
        {
			await _trainingPlanDataService.CreateTrainingPlan(trainingPlan);
			TempData["success"] = "Dodano plan!";
        }
        else
        {
			await _trainingPlanDataService.UpdateTrainingPlan(trainingPlan, _sessionPreviousExercises);
			TempData["success"] = "Zaktualizowano plan!";
        }

        if (_sessionTrainerClientId[0] != "0" && _sessionTrainerClientId[1] != "0")
        {
            int trainerId = int.Parse(_sessionTrainerClientId[0]);
            int clientId = int.Parse(_sessionTrainerClientId[1]);
            int planId = await _trainingPlanDataService.GetTrainingPlanId(tp.Title, tp.Creator);

            await _trainingPlanDataService.SetTrainingPlanToClient(trainerId, clientId, planId);
        }

		return RedirectToAction("Index");
	}


	public async Task<IActionResult> Show(int id)
    {
        TrainingPlan trainingPlan = await _trainingPlanDataService.GetTrainingPlan(id);
        _sessionExercises = await _trainingPlanDataService.GetTrainingPlanExercises(trainingPlan.Exercises);
        _sessionTrainingPlan = trainingPlan;
        return View(trainingPlan);
    }


	[ClearSessionStrings]
	public async Task<IActionResult> DeleteTrainingPlan(int id)
    {
        // BŁĄD: przypisanych planów treningowych nie można usuwać - konflikt kluczy
        await _trainingPlanDataService.DeleteTrainingPlan(id);
        TempData["success"] = "Usunięto plan treningowy";
        return RedirectToAction("Index");
    }


    public IActionResult UpdateTrainingPlan() =>
        RedirectToAction("Upsert");


    public async Task<IActionResult> ExerciseSelectionAsync() =>
		View();
    

    public IActionResult IncrementExerciseSeries(int id)
    {
		_sessionTrainingPlan = _trainingPlanDataService.IncrementExerciseSeriesAndGetTrainingPlan(_sessionTrainingPlan, id);
		return RedirectToAction("Upsert");
	}


    public async Task<IActionResult> DeleteExercise(int listPosition)
    {
		_sessionTrainingPlan = await _trainingPlanDataService.DeleteExerciseAndGetTrainingPlan(_sessionTrainingPlan, listPosition);
		_sessionExercises = _trainingPlanDataService.DeleteExerciseAndGetExercisesList(_sessionExercises, listPosition);
        return RedirectToAction("Upsert");
    }


	public IActionResult DecrementExerciseSeries(int id)
    {
		_sessionTrainingPlan = _trainingPlanDataService.DecrementExerciseSeriesAndGetTrainingPlan(_sessionTrainingPlan, id);
		return RedirectToAction("Upsert");
	}


    [HttpGet]
    public IActionResult SaveRepsAndWeightsData(string values, string exerciseId, string seriesPosition)
    {
	    _sessionTrainingPlan = _trainingPlanDataService.SaveRepsWeightsAndGetTrainingPlan(_sessionTrainingPlan, values, exerciseId, seriesPosition);
	    return Ok();
    }


	public async Task<IActionResult> AddExerciseId(int id)
    {
		_sessionTrainingPlan = await _trainingPlanDataService.AddExerciseAndGetTrainingPlan(_sessionTrainingPlan, id);
		_sessionExercises = await _trainingPlanDataService.AddExerciseAndGetExercisesList(_sessionExercises, id);
	    return RedirectToAction("Upsert");
	}


    public IActionResult SaveTitleData(string title)
    {
        _sessionTrainingPlan = _trainingPlanDataService.SaveTitleAndGetTrainingPlan(_sessionTrainingPlan, title);
        return Ok();
    }


    public IActionResult SaveTrainigDaysData(string day)
    {
		_sessionTrainingPlan = _trainingPlanDataService.SaveTrainingDaysAndGetTrainingPlan(_sessionTrainingPlan, day);
		return Ok();
    }


    public IActionResult SaveNotesData(string notes)
    {
        _sessionTrainingPlan = _trainingPlanDataService.SaveNotesAndGetTrainingPlan(_sessionTrainingPlan, notes);
        return Ok();
    }
}
