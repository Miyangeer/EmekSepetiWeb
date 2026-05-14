using Microsoft.AspNetCore.Identity;

namespace EmekSepeti.Models
{
    public class UygulamaKullanicisi : IdentityUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}