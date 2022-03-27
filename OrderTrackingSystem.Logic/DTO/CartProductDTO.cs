using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CartProductDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Cena { get; set; }
        [DisplayName("Ilość")]
        public string Amount { get; set; }
        [Browsable(false)]
        public decimal Rabat { get; set; }
    }
}
