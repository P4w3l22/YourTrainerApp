using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YourTrainerApp.Attributes;

public class AdminSessionCheck : ActionFilterAttribute
{
    private readonly string _sessionKey = "Username";
    private readonly string _adminEmail = "admin@gmail.com";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;

        if (session is null || 
            session.GetString(_sessionKey) != _adminEmail)
        {
            context.Result = new StatusCodeResult(403);
            return;
        }

        base.OnActionExecuting(context);
    }
}
