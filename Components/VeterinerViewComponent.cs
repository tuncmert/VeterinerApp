using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Components
{
    [ViewComponent]
    public class VeterinerViewComponent : ViewComponent
    {
        private readonly VeterinerDBContext _context;
        public VeterinerViewComponent(VeterinerDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginId = HttpContext.Session.GetString("vetloginId");
            try
            {
                var loginIdInt = Convert.ToInt32(loginId);
                var Veteriner = _context.Veteriners.FirstOrDefault(a=>a.VeterinerId==loginIdInt);
                return View(Veteriner);

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