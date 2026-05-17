using System;
using System.ComponentModel.DataAnnotations;

namespace EmekSepetiWeb.Models
{
    public class Siparis
    {
        public int Id { get; set; }
        public string UygulamaKullanicisiId { get; set; } // Siparişi veren müşteri
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;
        public decimal ToplamTutar { get; set; }

        [Required]
        public string TeslimatTuru { get; set; } // "Kargo" veya "Elden"

        public string? TeslimatAdresi { get; set; } // Kargo seçilirse zorunlu, elden seçilirse boş kalabilir
    }
}