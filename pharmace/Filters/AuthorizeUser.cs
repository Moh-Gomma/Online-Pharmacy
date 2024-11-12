using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HUc.Filters
{
    public class AuthorizeUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var svc = context.HttpContext.RequestServices;

            // TODO Handle casting error


            string userId = context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;




            if (userId == null)
            {
                var controller = context.Controller as ControllerBase;
                context.Result = controller.RedirectToAction("create", "user");
                return;
            }

            return;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
