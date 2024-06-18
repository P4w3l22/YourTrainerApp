using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourTrainer_Utility;
using YourTrainerApp.Areas.GymMember.Models;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Areas.GymMember.Controllers;

[Area("GymMember")]
public class TrainerContactController : Controller
{
	private readonly ITrainerDataService _trainerDataService;
	private readonly IMemberDataService _memberDataService;
	private readonly ITrainerClientContactService _trainerClientContactService;
	private int _memberId
	{
		get => int.Parse(HttpContext.Session.GetString("UserId"));
	}

    public TrainerContactController(ITrainerDataService trainerDataService, IMemberDataService memberDataService, ITrainerClientContactService trainerClientContactService)
    {
		_trainerDataService = trainerDataService;
		_memberDataService = memberDataService;
		_trainerClientContactService = trainerClientContactService;
	}

	public async Task<IActionResult> Index()
	{
		MemberDataModel memberData = await GetMemberData();

		if (!memberData.TrainersId.IsNullOrEmpty() && memberData.TrainersId != "0")
		{
			return RedirectToAction("TrainerDetails", "TrainerContact", new { Area = "GymMember", id = int.Parse(memberData.TrainersId) });
		}

		APIResponse apiResponse = await _trainerDataService.GetAllAsync<APIResponse>();

		List<TrainerDataModel> trainersData = JsonConvert.DeserializeObject<List<TrainerDataModel>>(Convert.ToString(apiResponse.Result));

		return View(trainersData);
	}

	public async Task<IActionResult> TrainerDetails(int id)
	{
		TrainerContact trainerContact = new();
		trainerContact.TrainerData = await SetTrainerData(id);
		trainerContact.MessagesWithTrainer = await SetMessagesWithTrainer(id);

		return View(trainerContact);
	}

	public async Task<IActionResult> TrainerMessages()
	{
		
		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	public async Task<IActionResult> AddTrainer(int id) 
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(id);
		TrainerDataModel trainerData = JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));

		if (trainerData.MembersId == "0" || trainerData.MembersId.IsNullOrEmpty())
		{
			trainerData.MembersId = _memberId.ToString();
		}
        else
        {
			bool memberExists = Array.Exists(trainerData.MembersId.Split(";"), memberId => memberId == _memberId.ToString());
			if (!memberExists)
			{
                trainerData.MembersId += ";" + _memberId.ToString();
            }
        }

		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);


		apiResponse = await _memberDataService.GetAsync<APIResponse>(_memberId);
		MemberDataModel memberData = JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));

		if (memberData.TrainersId == "0" || memberData.TrainersId.IsNullOrEmpty())
		{
			memberData.TrainersId = id.ToString();
		}
		else
		{
            bool trainerExists = Array.Exists(memberData.TrainersId.Split(";"), trainerId => trainerId == id.ToString());
			if (!trainerExists)
			{
                memberData.TrainersId += ";" + id.ToString();
            }
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

	public async Task<IActionResult> DeleteTrainerAsync()
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(_memberId);
		MemberDataModel memberData = JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));

		int trainerId = int.Parse(memberData.TrainersId);
		memberData.TrainersId = "";

		await _memberDataService.UpdateAsync<APIResponse>(memberData);


		apiResponse = await _trainerDataService.GetAsync<APIResponse>(trainerId);
		TrainerDataModel trainerData = JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));

		List<string> membersIdTrainer = trainerData.MembersId.Split(";").ToList();
		membersIdTrainer.RemoveAll(id => id == _memberId.ToString());
		trainerData.MembersId = String.Join(";", membersIdTrainer);

		await _trainerDataService.UpdateAsync<APIResponse>(trainerData);

		return RedirectToAction("Index", "TrainerContact", new { Area = "GymMember" });
	}

	private async Task<MemberDataModel> GetMemberData()
	{
		APIResponse apiResponse = await _memberDataService.GetAsync<APIResponse>(_memberId);
		return JsonConvert.DeserializeObject<MemberDataModel>(Convert.ToString(apiResponse.Result));
	}

	private async Task<TrainerDataModel> SetTrainerData(int id)
	{
		APIResponse apiResponse = await _trainerDataService.GetAsync<APIResponse>(id);
		return JsonConvert.DeserializeObject<TrainerDataModel>(Convert.ToString(apiResponse.Result));
	}
    private async Task<List<TrainerClientContact>> SetMessagesWithTrainer(int trainerId)
    {
		APIResponse apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(_memberId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact> memberMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));
    
		apiResponse = await _trainerClientContactService.GetMessagesAsync<APIResponse>(trainerId, StaticDetails.MessageType.Text.ToString());
		List<TrainerClientContact> trainerMessages = JsonConvert.DeserializeObject<List<TrainerClientContact>>(Convert.ToString(apiResponse.Result));

		return memberMessages.Concat(trainerMessages).OrderBy(m => m.SendDateTime).ToList();
	}
}
