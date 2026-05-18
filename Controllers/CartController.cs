using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmekSepetiWeb.Data;
using EmekSepetiWeb.Models;
using EmekSepeti.Models;
using System.Security.Claims;

namespace EmekSepetiWeb.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly UserManager<UygulamaKullanicisi> _userManager;

        public CartController(UygulamaDbContext context, UserManager<UygulamaKullanicisi> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kullaniciId = _userManager.GetUserId(User);

            // 1. Kullanıcının sepetindeki elemanları ham list olarak çekiyoruz
            var sepetElemanlari = await _context.SepetElemanlari
                .Where(s => s.UygulamaKullanicisiId == kullaniciId)
                .ToListAsync();

            var sepetListesi = new List<CartItemViewModel>();

            // 2. Ürünleri kimin eklediğine bakmaksızın, tamamen bağımsız bir şekilde ViewModel'e dolduruyoruz
            foreach (var eleman in sepetElemanlari)
            {
                var urun = await _context.Urunler.FirstOrDefaultAsync(u => u.Id == eleman.UrunId);
                if (urun != null)
                {
                    sepetListesi.Add(new CartItemViewModel
                    {
                        SepetElemanId = eleman.Id,
                        UrunId = urun.Id,
                        UrunAdi = urun.Ad, // Burası veritabanında 'Ad' olduğu için böyle bıraktım
                        Fiyat = urun.Fiyat,
                        Adet = eleman.Adet
                    });
                }
            }

            return View(sepetListesi);
        }

        // 2. SEPETE ÜRÜN EKLEME FONKSİYONU
        [HttpPost]
        public async Task<IActionResult> SepeteEkle(int urunId, int adet = 1)
        {
            var kullaniciId = _userManager.GetUserId(User);

            var urunVarMi = await _context.Urunler.AnyAsync(u => u.Id == urunId);
            if (!urunVarMi)
            {
                return NotFound("Ürün bulunamadı!");
            }

            var mevcutEleman = await _context.SepetElemanlari
                .FirstOrDefaultAsync(s => s.UygulamaKullanicisiId == kullaniciId && s.UrunId == urunId);

            if (mevcutEleman != null)
            {
                mevcutEleman.Adet += adet;
            }
            else
            {
                var yeniEleman = new SepetElemani
                {
                    UygulamaKullanicisiId = kullaniciId,
                    UrunId = urunId,
                    Adet = adet
                };
                _context.SepetElemanlari.Add(yeniEleman);
            }

            await _context.SaveChangesAsync();

            // Ekledikten sonra yukarıdaki Index metoduna yönlendiriyoruz
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int sepetElemanId)
        {
            // Projenizde alt çizgili (_context) mi yoksa çizgisi (context) mi kullanıldığını kontrol edin.
            // Eğer constructor'da alt çizgili tanımlandıysa bu kod hatasız çalışacaktır.
            var eleman = await _context.SepetElemanlari.FindAsync(sepetElemanId);

            if (eleman != null)
            {
                _context.SepetElemanlari.Remove(eleman);
                await _context.SaveChangesAsync();
            }

            // RedirectToAction yerine doğrudan Index görünümünü çağıralım (Kafa karışıklığını önlemek için)
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AdetArtir(int sepetElemanId)
        {
            var eleman = await _context.SepetElemanlari.FindAsync(sepetElemanId);
            if (eleman != null)
            {
                eleman.Adet += 1; // Adeti 1 artırıyoruz
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AdetAzalt(int sepetElemanId)
        {
            var eleman = await _context.SepetElemanlari.FindAsync(sepetElemanId);
            if (eleman != null)
            {
                if (eleman.Adet > 1)
                {
                    eleman.Adet -= 1; // 1'den büyükse 1 azaltıyoruz
                }
                else
                {
                    // Eğer adet zaten 1 ise ve tekrar azaltılmak isteniyorsa ürünü sepetten tamamen siliyoruz
                    _context.SepetElemanlari.Remove(eleman);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // 1. TESLİMAT SEÇİM SAYFASINI GÖSTEREN METOD (GET)
        [HttpGet]
        public IActionResult TeslimatSec()
        {
            return View();
        }

        // 2. SİPARİŞİ TAMAMLAYAN METOD (POST)
        [HttpPost]
        public async Task<IActionResult> SiparisTamamla(string teslimatTuru, string teslimatAdresi)
        {
            var kullaniciId = _userManager.GetUserId(User);

            // Kullanıcının sepetindeki ürünleri buluyoruz
            var sepetElemanlari = await _context.SepetElemanlari
                .Where(s => s.UygulamaKullanicisiId == kullaniciId)
                .ToListAsync();

            if (!sepetElemanlari.Any())
            {
                return RedirectToAction("Index");
            }

            // Toplam tutarı hesaplamak için ürün fiyatlarını çekiyoruz
            decimal toplamTutar = 0;
            foreach (var eleman in sepetElemanlari)
            {
                var urun = await _context.Urunler.FindAsync(eleman.UrunId);
                if (urun != null)
                {
                    toplamTutar += urun.Fiyat * eleman.Adet;
                }
            }

            var yeniSiparis = new Siparis
            {
                UygulamaKullanicisiId = kullaniciId,
                SiparisTarihi = DateTime.Now,
                ToplamTutar = toplamTutar,
                TeslimatTuru = teslimatTuru,
                TeslimatAdresi = teslimatTuru == "Kargo" ? teslimatAdresi : "Elden Teslim Alınacak (Ortak Nokta)"
            };

            _context.Siparisler.Add(yeniSiparis);
            _context.SepetElemanlari.RemoveRange(sepetElemanlari);
            await _context.SaveChangesAsync();

            return View("SiparisBasarili", yeniSiparis);
        }

        [HttpGet]
        public async Task<IActionResult> Siparislerim()
        {
            var kullaniciId = _userManager.GetUserId(User);

            var gecmisSiparisler = await _context.Siparisler
                .Where(s => s.UygulamaKullanicisiId == kullaniciId)
                .OrderByDescending(s => s.SiparisTarihi)
                .ToListAsync();

            return View(gecmisSiparisler);
        }
    }



}