using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class OrderDTO
    {
        public string Numer { get; set; }
        [DisplayName("Typ opłaty")]
        public string Oplata { get; set; }
        [DisplayName("Typ dostawy")]
        public string Dostawa { get; set; }
        public string Rezygnacja { get; set; }
        public string Kwota { get; set; }
        public string Sklep { get; set; }
    }
}
