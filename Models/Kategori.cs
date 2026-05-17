namespace EmekSepetiWeb.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Ad { get; set; } // Örn: "Yiyecek", "Tekstil", "Oyuncak"

        // Bir kategoride birden fazla ürün olabilir (İlişki kuruyoruz)
        public List<Urun> Urunler { get; set; } = new List<Urun>();
    }
}
