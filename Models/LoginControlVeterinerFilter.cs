using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Veterinerapp.Models
{
    public class LoginControlVeterinerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var loginId = context.HttpContext.Session.GetString("vetloginId");
            if(string.IsNullOrEmpty(loginId)){
                //Admin giriş yapmamış
                context.HttpContext.Response.Redirect("/Home/VetLogin");
            }
            base.OnActionExecuting(context);
        }
    }
}