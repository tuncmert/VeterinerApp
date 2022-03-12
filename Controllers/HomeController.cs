using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Veterinerapp.Models;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VeterinerDBContext _context;
        public HomeController(ILogger<HomeController> logger, VeterinerDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            if (login == null) return RedirectToAction("Login");
            var kullanici = _context.PetSahips.FirstOrDefault(p => p.KullaniciAdi == login.kullaniciAdi && p.SahipSifre == login.sifre);
            if (kullanici != null)
            {
                ViewBag.KullaniciMessage = "Girilen Bilgiler Doğru";
                HttpContext.Session.SetString("loginId", kullanici.Id.ToString());
                return RedirectToAction("KullaniciIndex", "Kullanici");
            }
            else
            {
                ViewBag.KullaniciMessage = "Kullanıcı adı ve şifrenizi kontrol edin";
            }
            return View();
        }
        public IActionResult VetLogin()
        {
            return View();
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(LoginModel login)
        {
            if (login == null) return RedirectToAction("AdminLogin");
            var admin = _context.Admins.FirstOrDefault(a => a.KullaniciAdi == login.kullaniciAdi && a.Sifre == login.sifre);
            if (admin != null)
            {
                ViewBag.AdminMessage = "Girilen Bilgiler Doğru";
                HttpContext.Session.SetString("adminloginId", admin.Id.ToString());
                return RedirectToAction("AdminIndex", "Admin");
            }
            else
            {
                ViewBag.AdminMessage = "Kullanıcı adı ve şifrenizi kontrol edin";
            }
            return View();
        }
                [HttpPost]
        public IActionResult VetLogin(VetLoginModel login)
        {
            if (login == null) return RedirectToAction("Login");
            var Veteriner = _context.Veteriners.FirstOrDefault(v => v.VeterinerKullaniciAdi == login.VeterinerKullaniciAdi && v.VeterinerSifre == login.VeterinerSifre);
            if (Veteriner != null)
            {
                //Giriş başarılı. 
                ViewBag.Message = "Giriş Başarılı";
                HttpContext.Session.SetString("vetloginId", Veteriner.VeterinerId.ToString());
                return RedirectToAction("VeterinerIndex", "Veteriner");
                //ViewComponent
            }
            else
            {
                ViewBag.Message = "Kullanıcı adı veya şifreniz hatalı";
            }
            return View();
        }

        public IActionResult Index()
        {
            ViewBag.kurumSayisi = _context.Kurums.Count();
            ViewBag.veterinerSayisi = _context.Veteriners.Count();
            ViewBag.petSayisi = _context.Pets.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult YeniBirAction()
        {
            return View();
        }
    }
}
