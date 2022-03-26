using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class ProductDTO
    {
        public string Nazwa { get; set; }
        [DisplayName("Cena netto")]
        public string Netto { get; set; }
        public string VAT { get; set; }
        public string Sprzedawca { get; set; }
        public string Kategoria { get; set; }
        public string Rabat { get; set; }
    }
}
