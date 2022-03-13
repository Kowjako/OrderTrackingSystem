using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class LocalizationDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Kraj { get; set; }
        public string Miasto { get; set; }
        public string Ulica { get; set; }
        public int Mieszkanie { get; set; }
        public int Budynek { get; set; }
        [DisplayName("Kod pocztowy")]
        public string Kod { get; set; }
    }
}
