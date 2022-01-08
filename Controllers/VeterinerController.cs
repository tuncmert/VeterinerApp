using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Veterinerapp.Models;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Controllers
{
    [LoginControlVeterinerFilter]
    public class VeterinerController : Controller
    {
        private readonly VeterinerDBContext _context;
        public VeterinerController(VeterinerDBContext context)
        {
            _context = context;
        }

        public IActionResult VeterinerIndex(int? id)
        {
            var VeterinerId = HttpContext.Session.GetString("vetloginId");
            var VeterinerIdInt = Convert.ToInt32(VeterinerId);
            var veteriner = _context.Veteriners.Include(o => o.Kurum).Include(o => o.Pets).ThenInclude(o => o.PetSahip).FirstOrDefault(o => o.VeterinerId == VeterinerIdInt);
            var VeterinerModel = new VeterinerModel();
            VeterinerModel.SeciliVeteriner = veteriner;
            VeterinerModel.TumSahipler = _context.PetSahips.ToList();
            ViewBag.petSayisi = _context.Pets.Count();
            return View(VeterinerModel);
        }
        [HttpPost]
        public IActionResult PetSahipSilAjax([FromBody] PetSahip petsahip)
        {
            if (petsahip == null) return Json("0");
            var silinecekUrun = _context.PetSahips.FirstOrDefault(p => p.Id == petsahip.Id);
            if (silinecekUrun == null) return Json("0");
            _context.Pets.RemoveRange(silinecekUrun.Pets);
            _context.SaveChanges();
            _context.PetSahips.Remove(silinecekUrun);
            _context.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public IActionResult PetSilAjax([FromBody] Pet pet)
        {
            if (pet == null) return Json("0");
            var silinecekUrun = _context.Pets.FirstOrDefault(p => p.Id == pet.Id);
            if (silinecekUrun == null) return Json("0");
            _context.Pets.Remove(silinecekUrun);
            _context.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public IActionResult KullaniciEkleAjax([FromBody] PetSahip petsahip)
        {
            if (string.IsNullOrEmpty(petsahip.SahipAd) || string.IsNullOrEmpty(petsahip.SahipSoyad)) return Json("0");
            _context.PetSahips.Add(petsahip);
            _context.SaveChanges();
            return Json("1");
        }
        public IActionResult PetDetay()
        {
            return View();
        }
        public IActionResult PetIndex(int? id)
        {
            var pet = _context.Pets.Include(p => p.Veteriner).Include(p => p.PetSahip).Include(p => p.Tedavis).Include(p => p.Asis).FirstOrDefault(p => p.Id == id);
            return View(pet);
        }
        public IActionResult PetSahipAjax([FromBody] PetSahip petsahip)
        {
            if (petsahip == null) return Json ("0");
            var seciliSahip = _context.PetSahips.FirstOrDefault(p => p.Id == petsahip.Id);
            return Json(seciliSahip);
        }
        public IActionResult PetEkleAjax([FromBody] Pet pet)
        {
            if (pet == null) return Json("0");
            _context.Pets.Add(pet);
            _context.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public async Task<IActionResult> PetDuzenleAsync()
        {
            var file = HttpContext.Request.Form.Files.FirstOrDefault();
            var petId = Convert.ToInt32(HttpContext.Request.Form["Id"]);
            var guncellenecekPet = _context.Pets.FirstOrDefault(p => p.Id == petId);
            guncellenecekPet.Kilo = HttpContext.Request.Form["Kilo"].ToString();
            guncellenecekPet.Tur = HttpContext.Request.Form["Tur"].ToString();
            guncellenecekPet.DogumTarihi = Convert.ToDateTime(HttpContext.Request.Form["DogumTarihi"]);
            _context.SaveChanges();
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    guncellenecekPet.Foto = fileBytes;
                    _context.SaveChanges();
                    var dosyaAdi = file.FileName;
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", dosyaAdi), fileBytes);
                }
            }
            return Json("1");
        }
        [HttpPost]
        public IActionResult AsiEkle([FromBody] Asi asi)
        {
            if (asi == null) return Json("0");
            _context.Asis.Add(asi);
            _context.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public IActionResult TedaviEkle([FromBody] Tedavi tedavi)
        {
            if (tedavi == null) return Json("0");
            _context.Tedavis.Add(tedavi);
            _context.SaveChanges();
            return Json("1");
        }
        public IActionResult IslemDetayAjax([FromBody] Tedavi tedavi)
        {
            var seciliIslem = _context.Tedavis.FirstOrDefault(t=>t.Id==tedavi.Id);
            return Json (seciliIslem);
        }
        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }
    }
}