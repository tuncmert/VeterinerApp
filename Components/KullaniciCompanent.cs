using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Components
{
    [ViewComponent]
    public class KullaniciViewComponent : ViewComponent
    {
        private readonly VeterinerDBContext _context;
        public KullaniciViewComponent(VeterinerDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginId = HttpContext.Session.GetString("loginId");
            try
            {
                var loginIdInt = Convert.ToInt32(loginId);
                var Kullanici = _context.PetSahips.FirstOrDefault(a=>a.Id==loginIdInt);
                return View(Kullanici);

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