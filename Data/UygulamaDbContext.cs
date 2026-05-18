using EmekSepeti.Models;
using EmekSepetiWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmekSepetiWeb.Data
{
    public class UygulamaDbContext : IdentityDbContext<UygulamaKullanicisi>
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

        public DbSet<Urun> Urunler { get; set; } // Bu satır ProductController için hayati!
        public DbSet<SepetElemani> SepetElemanlari { get; set; }

        public DbSet<Siparis> Siparisler { get; set; }



        public DbSet<Kategori> Kategoriler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kategori>().HasData(
                new Kategori { Id = 1, Ad = "Ev Yemekleri & Gıda" },
                new Kategori { Id = 2, Ad = "Takı & Aksesuar" },
                new Kategori { Id = 3, Ad = "Ahşap & Oyuncak" }
            );

            modelBuilder.Entity<Urun>().HasData(
                // Hepsine örnek de olsa bir Aciklama değeri ekledik:
                new Urun { Id = 1, Ad = "Kayseri Usulü Mantı", Aciklama = "Ev yapımı nefis Kayseri mantısı", Fiyat = 350.00m, ResimUrl = "manti.jpg", KategoriId = 1, OlusturmaTarihi = new DateTime(2026, 05, 17) },
                new Urun { Id = 2, Ad = "Doğal Taşlı Kolye", Aciklama = "Özel tasarım doğal taşlı şık kolye", Fiyat = 190.00m, ResimUrl = "kolye.jpg", KategoriId = 2, OlusturmaTarihi = new DateTime(2026, 05, 17) },
                new Urun { Id = 3, Ad = "Ahşap Oyuncak Tren", Aciklama = "Çocuklar için sağlıklı ahşap oyuncak tren", Fiyat = 280.00m, ResimUrl = "tren.jpg", KategoriId = 3, OlusturmaTarihi = new DateTime(2026, 05, 17) },
                new Urun { Id = 8, Ad = "El Örgüsü Atkı", Aciklama = "Gri ve günlük kullanıma uygun el örgüsü atkı", Fiyat = 149.99m, ResimUrl = "el örgüsü atkı.png", KategoriId = 2, OlusturmaTarihi = new DateTime(2026, 05, 17) },
                new Urun { Id = 9, Ad = "Tahta Pinokyo", Aciklama = "Ürün tahtadan yapılmıştır el emeğidir", Fiyat = 500.00m, ResimUrl = "tahta pinokyo.png", KategoriId = 3, OlusturmaTarihi = new DateTime(2026, 05, 17) }
            );
        }
    }
}