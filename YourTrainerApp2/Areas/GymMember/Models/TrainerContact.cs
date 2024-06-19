using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainer_App.Services.APIServices;
using YourTrainer_App.Services.APIServices.IServices;

namespace YourTrainerApp.Areas.GymMember.Models;

public class TrainerContact
{
	public TrainerDataModel TrainerData { get; set; }
	public List<TrainerClientContact> MessagesWithTrainer { get; set; }
    public List<TrainerClientContact> PlansFromTrainer { get; set; }

}