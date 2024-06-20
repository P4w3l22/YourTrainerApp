using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace YourTrainerApp.Attributes;

public class ClearSessionStrings : ActionFilterAttribute
{
	public override void OnActionExecuted(ActionExecutedContext context)
	{
		context.HttpContext.Session.SetString("TrainingPlanData", "");
		context.HttpContext.Session.SetString("Exercises", "");
		context.HttpContext.Session.SetString("PreviousExercises", JsonConvert.SerializeObject(new List<int>()));
		base.OnActionExecuted(context);
	}
}
