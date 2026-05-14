using EmekSepeti.Models;
using EmekSepetiWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EmekSepetiWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UygulamaKullanicisi> _userManager;
        private readonly SignInManager<UygulamaKullanicisi> _signInManager;

        public AccountController(UserManager<UygulamaKullanicisi> userManager, SignInManager<UygulamaKullanicisi> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- GİRİŞ YAP ---
        [HttpGet]
        public IActionResult GirisYap() => View();

        [HttpPost]
        public async Task<IActionResult> GirisYap(string Email, string Sifre, bool BeniHatirla)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Sifre))
            {
                ModelState.AddModelError("", "E-posta ve şifre boş bırakılamaz.");
                return View();
            }

            var kullanici = await _userManager.FindByEmailAsync(Email);
            if (kullanici != null)
            {
                var sonuc = await _signInManager.PasswordSignInAsync(Email, Sifre, BeniHatirla, lockoutOnFailure: false);
                if (sonuc.Succeeded)
                {
                    var claims = await _userManager.GetClaimsAsync(kullanici);
                    if (!claims.Any(c => c.Type == "TamAd"))
                    {
                        await _userManager.AddClaimAsync(kullanici, new Claim("TamAd", kullanici.Ad + " " + kullanici.Soyad));
                    }
                    await _signInManager.RefreshSignInAsync(kullanici);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Geçersiz e-posta veya şifre denemesi.");
            return View();
        }

        // --- KAYIT OL ---
        [HttpGet]
        public IActionResult KayitOl() => View();

        [HttpPost]
        public async Task<IActionResult> KayitOl(string Ad, string Soyad, string Email, string Sifre, string Telefon)
        {
            if (string.IsNullOrEmpty(Ad) || string.IsNullOrEmpty(Soyad) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Sifre))
            {
                ModelState.AddModelError("", "Lütfen gerekli alanları doldurun.");
                return View();
            }

            if (!string.IsNullOrEmpty(Telefon) && !Regex.IsMatch(Telefon, @"^05[0-9]{9}$"))
            {
                ModelState.AddModelError("", "Telefon numarası 05 ile başlamalı ve 11 haneli olmalıdır.");
                return View();
            }

            var yeniKullanici = new UygulamaKullanicisi
            {
                UserName = Email,
                Email = Email,
                Ad = Ad,
                Soyad = Soyad,
                PhoneNumber = Telefon,
                KayitTarihi = DateTime.Now
            };

            var sonuc = await _userManager.CreateAsync(yeniKullanici, Sifre);
            if (sonuc.Succeeded)
            {
                await _userManager.AddClaimAsync(yeniKullanici, new Claim("TamAd", Ad + " " + Soyad));
                await _signInManager.SignInAsync(yeniKullanici, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var hata in sonuc.Errors)
            {
                ModelState.AddModelError("", hata.Description);
            }
            return View();
        }

        // --- ŞİFREMİ UNUTTUM (TELEFONLA KONTROL) ---
        [HttpGet]
        public IActionResult SifremiUnuttum() => View();

        [HttpPost]
        public IActionResult SifremiUnuttum(string TelefonNo)
        {
            if (string.IsNullOrEmpty(TelefonNo) || !Regex.IsMatch(TelefonNo, @"^05[0-9]{9}$"))
            {
                ModelState.AddModelError("", "Geçersiz telefon numarası! 05XX XXX XX XX formatında olmalıdır.");
                return View();
            }

            var kullanici = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == TelefonNo);
            if (kullanici == null)
            {
                ModelState.AddModelError("", "Bu telefon numarası ile kayıtlı kullanıcı bulunamadı.");
                return View();
            }

            Random rand = new Random();
            string smsKodu = rand.Next(100000, 999999).ToString();

            TempData["SmsKodu"] = smsKodu;
            TempData["KullaniciId"] = kullanici.Id;

            return View("KodDogrula");
        }

        // --- KOD DOĞRULAMA Vb. ---
        [HttpPost]
        public async Task<IActionResult> KodDogrula(string GirilenKod, string YeniSifre)
        {
            string saklananKod = TempData["SmsKodu"] as string;
            string kullaniciId = TempData["KullaniciId"] as string;
            TempData.Keep();

            if (saklananKod != GirilenKod || string.IsNullOrEmpty(kullaniciId))
            {
                ViewBag.Hata = "Girdiğiniz onay kodu hatalı!";
                return View("KodDogrula");
            }

            var kullanici = await _userManager.FindByIdAsync(kullaniciId);
            if (kullanici != null)
            {
                await _userManager.RemovePasswordAsync(kullanici);
                await _userManager.AddPasswordAsync(kullanici, YeniSifre);
                return RedirectToAction("GirisYap");
            }

            ViewBag.Hata = "Bir hata oluştu.";
            return View("KodDogrula");
        }

        public async Task<IActionResult> CikisYap()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}