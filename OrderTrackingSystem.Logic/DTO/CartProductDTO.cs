using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CartProductDTO
    {
        public string Nazwa { get; set; }
        public string Cena { get; set; }
        [DisplayName("Ilość")]
        public string Amount { get; set; }
    }
}
