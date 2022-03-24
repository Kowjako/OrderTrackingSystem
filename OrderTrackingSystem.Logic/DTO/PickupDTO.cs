using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class PickupDTO
    {
        [DisplayName("Pojemność")]
        public string Capacity { get; set; }
        public string Adres { get; set; }
        public string MiastoKod { get; set; }
        [DisplayName("Czas pracy")]
        public string WorkTime { get; set; }
    }
}
