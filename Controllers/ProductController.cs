using EmekSepeti.Models;
using EmekSepetiWeb.Data;
using EmekSepetiWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmekSepetiWeb.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly UserManager<UygulamaKullanicisi> _userManager;

        public ProductController(UygulamaDbContext context, UserManager<UygulamaKullanicisi> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Urun urun, IFormFile? ResimDosyasi)
        {
            var kullaniciId = _userManager.GetUserId(User);
            urun.UygulamaKullanicisiId = kullaniciId;
            urun.OlusturmaTarihi = DateTime.Now;

            if (ResimDosyasi != null && ResimDosyasi.Length > 0)
            {
                var klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(klasorYolu))
                {
                    Directory.CreateDirectory(klasorYolu);
                }

                var benzersizDosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(ResimDosyasi.FileName);
                var tamDosyaYolu = Path.Combine(klasorYolu, benzersizDosyaAdi);

                using (var stream = new FileStream(tamDosyaYolu, FileMode.Create))
                {
                    await ResimDosyasi.CopyToAsync(stream);
                }

                urun.ResimUrl = "/images/" + benzersizDosyaAdi;
            }
            else
            {
                urun.ResimUrl = "https://images.unsplash.com/photo-1513519245088-0e12902e5a38?q=80&w=500";
            }

            _context.Urunler.Add(urun);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}