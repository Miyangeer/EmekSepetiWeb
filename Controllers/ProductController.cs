using EmekSepeti.Models;
using EmekSepetiWeb.Data;
using EmekSepetiWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        // ➕ ProductController.cs içindeki Urunlerim Metodu
        public async Task<IActionResult> Urunlerim()
        {
            // Giriş yapan kullanıcının ID'sini alıyoruz
            var kullaniciId = _userManager.GetUserId(User);

            // DÜZELTME: .Include(u => u.Kategori) ekleyerek numaradan isme geçiş köprüsünü kurduk!
            var benimUrunlerim = await _context.Urunler
                .Include(u => u.Kategori)
                .Where(u => u.UygulamaKullanicisiId == kullaniciId)
                .OrderByDescending(u => u.OlusturmaTarihi)
                .ToListAsync();

            return View(benimUrunlerim);
        }

        // ➕ ÜRÜN EKLEME SAYFASI (GET)
        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            // Veritabanındaki tüm kategorileri çekip View'a (Arayüze) gönderiyoruz
            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
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

                // DÜZELTME: Başına ekstra "/images/" eklemiyoruz, sadece dosya adını yazıyoruz!
                urun.ResimUrl = benzersizDosyaAdi;
            }
            else
            {
                // Resim seçilmezse Unsplash'ten gelen link direkt tam url olduğu için bu kalabilir.
                urun.ResimUrl = "https://images.unsplash.com/photo-1513519245088-0e12902e5a38?q=80&w=500";
            }

            _context.Urunler.Add(urun);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // 🗑️ ÜRÜN SİLME İŞLEMİ
        [HttpGet]
        public async Task<IActionResult> Sil(int id)
        {
            // Giriş yapan kullanıcının ID'sini alıyoruz
            var kullaniciId = _userManager.GetUserId(User);

            // Silinmek istenen ürünü veritabanında buluyoruz
            var urun = await _context.Urunler.FirstOrDefaultAsync(u => u.Id == id);

            // Eğer ürün bulunamadıysa veya ürünün sahibi giriş yapan kişi değilse SİLDİRME!
            if (urun == null || urun.UygulamaKullanicisiId != kullaniciId)
            {
                return NotFound(); // Güvenlik duvarı: Yetkisiz erişimi engelledik
            }

            // Her şey doğruysa ürünü veritabanından kaldır
            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();

            // Silme işleminden sonra kullanıcıyı tekrar kendi ürün listesine gönder
            return RedirectToAction("Urunlerim");
        }

        // ✏️ GÜNCELLEME SAYFASINI AÇMA (GET)
        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var kullaniciId = _userManager.GetUserId(User);

            // Düzenlenmek istenen ürünü buluyoruz
            var urun = await _context.Urunler.FirstOrDefaultAsync(u => u.Id == id);

            // Güvenlik Kontrolü: Ürün yoksa veya sahibi bu giriş yapan kişi değilse açma!
            if (urun == null || urun.UygulamaKullanicisiId != kullaniciId)
            {
                return NotFound();
            }

            return View(urun); // Ürün bilgilerini form sayfasına gönderiyoruz
        }

        // ✏️ GÜNCELLEME İŞLEMİNİ KAYDETME (POST)
        [HttpPost]
        public async Task<IActionResult> Duzenle(int id, Urun guncelUrun)
        {
            if (id != guncelUrun.Id)
            {
                return NotFound();
            }

            var kullaniciId = _userManager.GetUserId(User);
            // Veritabanındaki orijinal ürünü çekiyoruz
            var orjinalUrun = await _context.Urunler.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            // Güvenlik Kontrolü
            if (orjinalUrun == null || orjinalUrun.UygulamaKullanicisiId != kullaniciId)
            {
                return NotFound();
            }

            try
            {
                // Kullanıcının değiştiremeyeceği, arka planda sabit kalması gereken bilgileri koruyoruz
                guncelUrun.UygulamaKullanicisiId = kullaniciId;
                guncelUrun.KategoriId = orjinalUrun.KategoriId; // Kategori hatası almamak için eski kategoriyi koruduk
                guncelUrun.ResimUrl = orjinalUrun.ResimUrl;     // Şimdilik eski resmi koruyoruz

                // Veritabanında güncelle ve kaydet
                _context.Update(guncelUrun);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return View(guncelUrun);
            }

            // İşlem bitince kullanıcının kendi ürün listesine geri dön
            return RedirectToAction("Urunlerim");
        }
        // 📌 DETAILS METODUNU DA ASENKRON YAPTIK (Daha performanslı çalışması için)
        public async Task<IActionResult> Details(int id)
        {
            var urun = await _context.Urunler.FirstOrDefaultAsync(x => x.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }
    }
}