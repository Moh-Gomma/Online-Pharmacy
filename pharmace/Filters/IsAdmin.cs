using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace HUc.Filters
{
    public class IsAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        
        {

            var svc = context.HttpContext.RequestServices;



            string userId = context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;


            if (userId == null)
            {
                var controller = context.Controller as ControllerBase;
                context.Result = controller.RedirectToAction("create", "user");
                return;
            }

            string role = context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "role")?.Value;

            if (role == null || role != "admin")
            {
                var controller = context.Controller as ControllerBase;
                context.Result = controller.RedirectToAction("Index", "Home");
                return;
            }
            return;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
