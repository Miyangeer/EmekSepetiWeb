using EmekSepetiWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmekSepetiWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HomeController(UygulamaDbContext context)
        {
            _context = context;
        }

        // Ýki kodu birleþtirdiðimiz tek ve doðru Index metodu
        public async Task<IActionResult> Index(int? kategoriId)
        {
            // 1. Veritabanýndaki ürünleri ve ekleyen kullanýcýyý hazýrla
            var urunlerQuery = _context.Urunler
                .Include(u => u.UygulamaKullanicisi)
                .AsQueryable();

            // 2. Eðer bir kategoriye týklandýysa filtrele
            if (kategoriId.HasValue)
            {
                urunlerQuery = urunlerQuery.Where(u => u.KategoriId == kategoriId.Value);
            }

            // 3. Yeniden eskiye doðru sýrala ve listeyi ekrana gönder
            var urunler = await urunlerQuery
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