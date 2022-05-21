using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class PickupDTO
    {
        [Display(Name = "Capacity", ResourceType = typeof(Properties.Resources))]
        public string Capacity { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Properties.Resources))]
        public string Address { get; set; }

        [Display(Name = "City", ResourceType = typeof(Properties.Resources))]
        public string CityWithCode { get; set; }

        [Display(Name = "WorkTime", ResourceType = typeof(Properties.Resources))]
        public string WorkTime { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        #endregion
    }
}
