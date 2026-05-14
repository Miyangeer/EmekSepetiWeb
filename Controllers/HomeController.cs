using EmekSepetiWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmekSepetiWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HomeController(UygulamaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Veritabanýndaki ürünleri, ekleyen kullanýcý bilgisiyle birlikte çekiyoruz
            var urunler = await _context.Urunler
                .Include(u => u.UygulamaKullanicisi)
                .OrderByDescending(u => u.OlusturmaTarihi)
                .ToListAsync();

            return View(urunler);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}