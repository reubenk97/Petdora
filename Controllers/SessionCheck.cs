using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Petdora.Controllers;

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "User", null);
        }
    }
}