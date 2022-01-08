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
    [LoginControlAdminFilter]
    public class AdminController : Controller
    {
        private readonly VeterinerDBContext _context;

        public AdminController(VeterinerDBContext context)
        {
            _context = context;
        }

        public IActionResult AdminIndex()
        {
            var adminId = HttpContext.Session.GetString("loginId");
            var adminIdInt = Convert.ToInt32(adminId);
            var admin = _context.Admins.FirstOrDefault(a => a.Id == adminIdInt);

            ViewBag.kurumSayisi = _context.Kurums.Count(); ;
            ViewBag.veterinerSayisi = _context.Veteriners.Count(); ;
            ViewBag.kurumlar = _context.Kurums.ToList();
            ViewBag.petSayisi = _context.Pets.Count();
            return View(admin);
        }
        public IActionResult Kurumlar()
        {
            var tumKurumlar = _context.Kurums.ToList();
            return View(tumKurumlar);
        }
        [HttpPost]
        public IActionResult KurumEkleAjax([FromBody] Kurum kurum)
        {
            if (kurum == null) return Json("0");
            _context.Kurums.Add(kurum);
            _context.SaveChanges();
            return Json("1");
        }
        public IActionResult KurumIndex(int? id)
        {
            if (id == null) return RedirectToAction("AdminIndex");
            var seciliKurum = _context.Kurums.FirstOrDefault(o => o.KurumId == id);
            if (seciliKurum == null) return RedirectToAction("AdminIndex");
            var tumVeterinerler = _context.Veteriners.Include(o => o.Pets).Where(o => o.KurumId == id).ToList();
            var KurumVeterinerModel = new KurumVeterinerListesiModel();
            KurumVeterinerModel.TumVeterinerler = tumVeterinerler;
            KurumVeterinerModel.SeciliKurum = seciliKurum;
            return View(KurumVeterinerModel);
        }
        [HttpPost]
        public IActionResult KurumSilAjax([FromBody] Kurum kurum)
        {
            if (kurum == null) return Json("0");
            var silinecekKurum = _context.Kurums.Include(O => O.Veteriners).FirstOrDefault(o => o.KurumId == kurum.KurumId);
            if (silinecekKurum == null) return Json("0");
            _context.Veteriners.RemoveRange(silinecekKurum.Veteriners);
            _context.SaveChanges();
            _context.Kurums.Remove(silinecekKurum);
            _context.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public IActionResult KurumDuzenleAjax([FromBody] Kurum kurum)
        {
            if (kurum == null) return Json("0");
            var guncellecekKurum = _context.Kurums.FirstOrDefault(o => o.KurumId == kurum.KurumId);
            guncellecekKurum.KurumAd = kurum.KurumAd;
            guncellecekKurum.KurumIlce = kurum.KurumIlce;
            guncellecekKurum.KurumSehir = kurum.KurumSehir;
            guncellecekKurum.KurumTelefon = kurum.KurumTelefon;
            _context.SaveChanges();
            return Json("1");
        }
        public IActionResult Veterinerler()
        {
            var tumVeterinerler = _context.Veteriners.Include(o => o.Kurum).ToList();
            ViewBag.kurumlar = _context.Kurums.ToList();
            return View(tumVeterinerler);
        }
        [HttpPost]
        public IActionResult VeterinerEkleAjax([FromBody] Veteriner veteriner)
        {
            if (veteriner == null) return Json("0");
            _context.Veteriners.Add(veteriner);
            _context.SaveChanges();
            return Json("1");
        }
        public IActionResult VeterinerIndex(int? id)
        {
            var veteriner = _context.Veteriners.Include(o => o.Kurum).Include(o => o.Pets).ThenInclude(O => O.PetSahip).FirstOrDefault(o => o.VeterinerId == id);
            return View(veteriner);
        }
        // [HttpPost]
        // public IActionResult VeterinerDuzenleAjax([FromBody] Veteriner veteriner)
        // {
        //     if (veteriner == null) return Json("0");
        //     var guncellenecekVeteriner = _context.Veteriners.FirstOrDefault(o => o.VeterinerId == veteriner.VeterinerId);
        //     guncellenecekVeteriner.VeterinerAd = veteriner.VeterinerAd;
        //     guncellenecekVeteriner.VeterinerSoyad = veteriner.VeterinerSoyad;
        //     guncellenecekVeteriner.VeterinerTelefon = veteriner.VeterinerTelefon;
        //     guncellenecekVeteriner.VeterinerAktifMi = veteriner.VeterinerAktifMi;
        //     guncellenecekVeteriner.VeterinerKullaniciAdi = veteriner.VeterinerKullaniciAdi;
        //     guncellenecekVeteriner.VeterinerSifre = veteriner.VeterinerSifre;
        //     _context.SaveChanges();
        //     return Json("1");
        // }
        [HttpPost]
        public IActionResult VeterinerSilAjax([FromBody] Veteriner veteriner)
        {
            if (veteriner == null) return Json("0");
            var silinecekUrun = _context.Veteriners.FirstOrDefault(o => o.VeterinerId == veteriner.VeterinerId);
            if (silinecekUrun == null) return Json("0");
            var petler = _context.Pets.Where(o => o.VeterinerId == silinecekUrun.VeterinerId);
            foreach (var item in petler)
            {
                item.VeterinerId= null;
            }
            _context.SaveChanges();
            _context.Veteriners.Remove(silinecekUrun);
            _context.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public async Task<IActionResult> VeterinerFotoEkleAsync([FromForm] IList<IFormFile> dosya)
        {
            if (dosya == null) return Json("0");
            var file = HttpContext.Request.Form.Files.FirstOrDefault();
            var veterinerId = Convert.ToInt32(HttpContext.Request.Form["VeterinerId"]);
            var guncellenecekVeteriner = _context.Veteriners.FirstOrDefault(o => o.VeterinerId == veterinerId);
            guncellenecekVeteriner.VeterinerAd = HttpContext.Request.Form["VeterinerAd"].ToString();
            guncellenecekVeteriner.VeterinerSoyad = HttpContext.Request.Form["VeterinerSoyad"].ToString();
            guncellenecekVeteriner.VeterinerTelefon = HttpContext.Request.Form["VeterinerTelefon"].ToString();
            guncellenecekVeteriner.VeterinerKullaniciAdi = HttpContext.Request.Form["VeterinerKullaniciAdi"].ToString();
            guncellenecekVeteriner.VeterinerSifre = HttpContext.Request.Form["VeterinerSifre"].ToString();
            guncellenecekVeteriner.VeterinerAktifMi = Convert.ToBoolean(HttpContext.Request.Form["VeterinerAktifMi"]);
            _context.SaveChanges();
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    guncellenecekVeteriner.VeterinerImage = fileBytes;
                    _context.SaveChanges();
                    var dosyaAdi = file.FileName;
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", dosyaAdi), fileBytes);
                }
            }
            return Json("1");
        }
        public IActionResult Petler()
        {
            var tumPetler = _context.Pets.Include(o => o.PetSahip).Include(o => o.Veteriner).ToList();
            return View(tumPetler);
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AdminLogin","Home");
        }

    }
}