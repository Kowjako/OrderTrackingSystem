using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class PickupDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Display(Name = "Capacity", ResourceType = typeof(Properties.Resources))]
        public string Capacity { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Properties.Resources))]
        public string Adres { get; set; }
        [Display(Name = "City", ResourceType = typeof(Properties.Resources))]
        public string MiastoKod { get; set; }
        [Display(Name = "WorkTime", ResourceType = typeof(Properties.Resources))]
        public string WorkTime { get; set; }
    }
}
