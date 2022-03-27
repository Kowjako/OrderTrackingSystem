using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class PickupDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Pojemność")]
        public string Capacity { get; set; }
        [DisplayName("Adres")]
        public string Adres { get; set; }
        [DisplayName("Miasto")]
        public string MiastoKod { get; set; }
        [DisplayName("Czas pracy")]
        public string WorkTime { get; set; }
    }
}
