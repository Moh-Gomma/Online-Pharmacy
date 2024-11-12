using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

public class PreventLoggedInAccessAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
        base.OnActionExecuting(context);
    }
}