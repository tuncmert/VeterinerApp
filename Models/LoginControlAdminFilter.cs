using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Veterinerapp.Models
{
    public class LoginControlAdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var loginId = context.HttpContext.Session.GetString("adminloginId");
            if(string.IsNullOrEmpty(loginId)){
                //Admin giriş yapmamış
                context.HttpContext.Response.Redirect("/Home/AdminLogin");
            }
            base.OnActionExecuting(context);
        }
    }
}