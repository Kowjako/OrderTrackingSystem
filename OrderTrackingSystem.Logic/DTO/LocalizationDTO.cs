using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class LocalizationDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Display(Name = "Country", ResourceType = typeof(Properties.Resources))]
        public string Kraj { get; set; }
        [Display(Name = "City", ResourceType = typeof(Properties.Resources))]
        public string Miasto { get; set; }
        [Display(Name = "Street", ResourceType = typeof(Properties.Resources))]
        public string Ulica { get; set; }
        [Display(Name = "Apartment", ResourceType = typeof(Properties.Resources))]
        public int Mieszkanie { get; set; }
        [Display(Name = "House", ResourceType = typeof(Properties.Resources))]
        public int Budynek { get; set; }
        [Display(Name = "ZipCode", ResourceType = typeof(Properties.Resources))]
        public string Kod { get; set; }
    }
}
