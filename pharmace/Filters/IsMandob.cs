using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HUc.Filters
{
    public class IsUser : ActionFilterAttribute
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

            if (role == "admin")
                return;

            if (role == null || role != "user" )
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            return;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
