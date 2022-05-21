using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class LocalizationDTO
    {
        [Display(Name = "Country", ResourceType = typeof(Properties.Resources))]
        public string Country { get; set; }

        [Display(Name = "City", ResourceType = typeof(Properties.Resources))]
        public string City { get; set; }

        [Display(Name = "Street", ResourceType = typeof(Properties.Resources))]
        public string Street { get; set; }

        [Display(Name = "Apartment", ResourceType = typeof(Properties.Resources))]
        public int Apartment { get; set; }

        [Display(Name = "House", ResourceType = typeof(Properties.Resources))]
        public int House { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Properties.Resources))]
        public string ZipCode { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        #endregion
    }
}
