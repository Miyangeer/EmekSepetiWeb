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
    }
}