using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMemberDataService _memberDataService;
	private int _memberId
	{
		get => int.Parse(HttpContext.Session.GetString("UserId"));
	}

    public TrainerContactController(ITrainerDataService trainerDataService, IMemberDataService memberDataService)
    {
		_trainerDataService = trainerDataService;
		_memberDataService = memberDataService;
    }

    public async Task<IActionResult> Index()
	{
		APIResponse apiResponse = await _trainerDataService.GetAllAsync<APIResponse>();

		List<TrainerDataModel> trainersData = JsonConvert.DeserializeObject<List<TrainerDataModel>>(Convert.ToString(apiResponse.Result));

		return View(trainersData);
	}

	public IActionResult TrainerMessages()
	{
		return View();
	}

	public async Task<IActionResult> AddTrainer(int id) 
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(id);
		TrainerDataModel trainerData = JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));

		if (trainerData.MembersId == "0")
		{
			trainerData.MembersId = _memberId.ToString();
		}
        else
        {
			trainerData.MembersId += ";" + _memberId.ToString();
		}

		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);


		apiResponse = await _memberDataService.GetAsync<APIResponse>(_memberId);
		MemberDataModel memberData = JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));

		if (memberData.TrainersId == "0")
		{
			memberData.TrainersId = id.ToString();
		}
		else
		{
			memberData.TrainersId += ";" + id.ToString();
		}

		await _memberDataService.UpdateAsync<APIResponse>(memberData);

		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	public IActionResult TrainerSelection()
	{
		return View();
	}

	public IActionResult AssignedTrainingPlan()
	{
		return View();
	}

}
