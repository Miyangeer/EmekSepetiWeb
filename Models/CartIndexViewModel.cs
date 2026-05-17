namespace EmekSepetiWeb.Models
{
    public class CartItemViewModel
    {
        public int SepetElemanId { get; set; }
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public decimal Fiyat { get; set; }
        public int Adet { get; set; }
    }
}