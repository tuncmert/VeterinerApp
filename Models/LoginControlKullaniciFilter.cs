using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Veterinerapp.Models
{
    public class LoginControlKullaniciFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var loginId = context.HttpContext.Session.GetString("loginId");
            if (string.IsNullOrEmpty(loginId))
            {
                //Kullanıcı giriş yapmamış
                context.HttpContext.Response.Redirect("/Home/Login");
            }
            base.OnActionExecuting(context);
        }
    }
}