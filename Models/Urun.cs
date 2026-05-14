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
        public string ResimUrl { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        public string UygulamaKullanicisiId { get; set; }
        public UygulamaKullanicisi UygulamaKullanicisi { get; set; }
    }
}