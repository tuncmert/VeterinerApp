using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinerapp.Models;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Controllers
{
    [LoginControlKullaniciFilter]
    public class KullaniciController : Controller
    {
        private readonly VeterinerDBContext _context;
        public KullaniciController(VeterinerDBContext context)
        {
            _context = context;
        }
        public IActionResult KullaniciIndex()
        {
            var kullaniciId = HttpContext.Session.GetString("loginId");
            var kullaniciIdInt = Convert.ToInt32(kullaniciId);
            var kullanici = _context.PetSahips.Include(p => p.Pets).ThenInclude(p => p.Veteriner).FirstOrDefault(p => p.Id == kullaniciIdInt);
            return View(kullanici);
        }
        [HttpPost]
        public IActionResult KullaniciDuzenle([FromBody] PetSahip kullanici)
        {
            if (kullanici == null) return Json("0");
            var guncellenecekKullanici = _context.PetSahips.FirstOrDefault(k => k.Id == kullanici.Id);
            guncellenecekKullanici.SahipAd = kullanici.SahipAd;
            guncellenecekKullanici.SahipSoyad = kullanici.SahipSoyad;
            guncellenecekKullanici.SahipTelefon = kullanici.SahipTelefon;
            guncellenecekKullanici.SahipSifre = kullanici.SahipSifre;
            _context.SaveChanges();
            return Json("1");
        }
        public IActionResult PetIndex(int? id)
        {
            var pet = _context.Pets.Include(p => p.Veteriner).Include(p => p.PetSahip).FirstOrDefault(p => p.Id == id);
            var tedaviler = _context.Tedavis.Where(p => p.PetId == id).ToList();
            var asilar = _context.Asis.Where(p => p.PetId == id).ToList();
            var petTedaviModel = new PetTedaviAsiModel();
            petTedaviModel.SeciliPet = pet;
            petTedaviModel.Tedaviler = tedaviler;
            petTedaviModel.Asilar = asilar;
            return View(petTedaviModel);
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
        public IActionResult TedaviDetayAjax([FromBody] Tedavi tedavi)
        {
            if (tedavi == null) return Json('0');
            var seciliTedavi = _context.Tedavis.FirstOrDefault(t => t.Id == tedavi.Id);
            return Json(seciliTedavi);
        }
        public IActionResult AsiDetayAjax([FromBody] Asi asi)
        {
            if(asi==null) return Json("0");
            var seciliAsi =_context.Asis.FirstOrDefault(a=>a.Id==asi.Id);
            return Json(seciliAsi);
        }
    }
}