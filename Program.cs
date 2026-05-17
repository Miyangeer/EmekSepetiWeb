using EmekSepeti.Models;
using EmekSepetiWeb.Data;
using EmekSepetiWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantýsý (SQL Server)
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Ayarlarý
builder.Services.AddIdentity<UygulamaKullanicisi, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<UygulamaDbContext>()
.AddDefaultTokenProviders();

// Cookie/Oturum Ayarlarý
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/GirisYap";
    options.LogoutPath = "/Account/CikisYap";
    options.AccessDeniedPath = "/Home/Error";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- VERÝTABANI TOHUMLAMA (SEED DATA) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UygulamaDbContext>();
    context.Database.Migrate();

    if (!context.Kategoriler.Any())
    {
        var kategoriler = new List<Kategori>
    {
        new Kategori { Ad = "Ev Yemekleri & Gýda" },
        new Kategori { Ad = "Taký & Aksesuar" },
        new Kategori { Ad = "Ahþap & Oyuncak" }
    };
        context.Kategoriler.AddRange(kategoriler);
        context.SaveChanges(); // Önce kategorileri kaydet ki Id'leri oluþsun
    }

    if (!context.Urunler.Any())
    {
        // Artýk ürünleri eklerken KategoriId verebiliriz
        var urunler = new List<Urun>
    {
        new Urun { Ad = "Kayseri Usulü Ev Mantýsý (1 Kg)", Aciklama = "...", Fiyat = 350, KategoriId = 1, ResimUrl="manti.jpg" },
        new Urun { Ad = "Doðal Taþlý El Yapýmý Kolye", Aciklama = "...", Fiyat = 190, KategoriId = 2, ResimUrl="kolye.jpg" },
        new Urun { Ad = "Ahþap Oyuncak Tren Seti", Aciklama = "...", Fiyat = 280, KategoriId = 3, ResimUrl="tren.jpg" }
    };
        context.Urunler.AddRange(urunler);
        context.SaveChanges();
    }

    // Hata veren alanlarý kaldýrýp en sade ve güvenli haliyle tohumlama yapýyoruz
    if (!context.Urunler.Any())
    {
        var urunler = new List<Urun>
        {
            new Urun
            {
                Ad = "Kayseri Usulü Ev Mantýsý (1 Kg)",
                Aciklama = "%100 dana kýymasýyla, tamamen ev ortamýnda elde kesilerek hazýrlanmýþ dondurulmuþ anne mantýsý.",
                Fiyat = 350,
                ResimUrl = "mantý.jpg", // Boþ kalmasýn diye ekledik
                UygulamaKullanicisiId = null // Eðer modeli '?' ile yaptýysan null yazabilirsin
            },
            new Urun
            {
                Ad = "Doðal Taþlý El Yapýmý Kolye",
                Aciklama = "Gerçek ametist ve lav taþlarý kullanýlarak pirinç tel sarma tekniðiyle tasarlanmýþ eþsiz þans kolyesi.",
                Fiyat = 190,
                ResimUrl = "kolye.jpg",
                UygulamaKullanicisiId = null
            },
            new Urun
            {
                Ad = "Ahþap Oyuncak Tren Seti",
                Aciklama = "Çocuklar için tamamen doðal gürgen aðacýndan üretilmiþ, organik boyalý 5 parçalý harika bir oyuncak set.",
                Fiyat = 280,
                ResimUrl = "tren.jpg",
                UygulamaKullanicisiId = null
            }
        };

        context.Urunler.AddRange(urunler);
        context.SaveChanges();
    }
    }

    // --- ORTAM KONTROLÜ ---
    if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Route/Yönlendirme Tanýmý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();