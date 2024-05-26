using Microsoft.AspNetCore.Mvc.Filters;

namespace YourTrainerApp.Attributes;

public class AdminSessionCheckAttribute : ActionFilterAttribute
{

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;



        base.OnActionExecuting(context);
    }
}
