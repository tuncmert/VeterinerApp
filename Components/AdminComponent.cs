using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Components
{
    [ViewComponent]
    public class AdminViewComponent : ViewComponent
    {
        private readonly VeterinerDBContext _context;
        public AdminViewComponent(VeterinerDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginId = HttpContext.Session.GetString("adminloginId");
            try
            {
                var loginIdInt = Convert.ToInt32(loginId);
                var admin = _context.Admins.FirstOrDefault(a=>a.Id==loginIdInt);
                return View(admin);
                
            }
            catch (System.Exception)
            {
#if DEBUG              
                throw;
#endif
            }
        }

    }
}