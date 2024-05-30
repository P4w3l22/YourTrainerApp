using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace YourTrainerApp.Attributes;

public class ClearSessionStrings : ActionFilterAttribute
{
	public override void OnActionExecuted(ActionExecutedContext context)
	{
		context.HttpContext.Session.SetString("TrainingPlanData", "");
		context.HttpContext.Session.SetString("Exercises", "");
		base.OnActionExecuted(context);
	}
}
