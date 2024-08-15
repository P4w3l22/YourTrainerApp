using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_Utility;
using YourTrainer_App.Models;
using YourTrainer_App.Services.APIServices;
using YourTrainer_App.Services.APIServices.IServices;

namespace YourTrainer_App.Areas.GymMember.Models;

public class TrainerContact
{
	public TrainerDataModel TrainerData { get; set; }
	public List<TrainerClientContact> MessagesWithTrainer { get; set; }
    public List<TrainingPlan> PlansFromTrainer { get; set; }

}