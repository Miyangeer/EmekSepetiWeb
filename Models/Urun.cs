using EmekSepeti.Models;
using System;

namespace EmekSepetiWeb.Models
{
    public class Urun
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public decimal Fiyat { get; set; }
        public string? ResimUrl { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        public string? UygulamaKullanicisiId { get; set; } // Soru işareti boş kalabilir demek.
        public UygulamaKullanicisi UygulamaKullanicisi { get; set; }

        // Ürünün hangi kategoriye ait olduğunu tutacak olan Foreign Key
        public int KategoriId { get; set; }

        // Ürünün bağlı olduğu Kategori nesnesi
        public Kategori Kategori { get; set; }
    }
}