using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CustomerDTO
    {
        public string Nazwa { get; set; }
        public string NIP { get; set; }
        public string Email { get; set; }
        public string Numer { get; set; }
        public string Ulica { get; set; }
        public string Budynek { get; set; }
        public string Miasto { get; set; }
        [DisplayName("Kod pocztowy")]
        public string Kod { get; set; }
    }
}
