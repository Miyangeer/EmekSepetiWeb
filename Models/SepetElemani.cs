using EmekSepeti.Models;
using System.ComponentModel.DataAnnotations;

namespace EmekSepetiWeb.Models
{
    public class SepetElemani
    {
        [Key]
        public int Id { get; set; }

        // Sepetin hangi kullanıcıya ait olduğunu tutuyoruz
        public string UygulamaKullanicisiId { get; set; }
        public UygulamaKullanicisi UygulamaKullanicisi { get; set; }

        // Sepete hangi ürünün eklendiğini tutuyoruz
        public int UrunId { get; set; }
        public Urun Urun { get; set; }

        // Kaç adet eklendiğini tutuyoruz (Doğal ürünlerde adet/kilo mantığı için)
        public int Adet { get; set; } = 1;
    }
}
