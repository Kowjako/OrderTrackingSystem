using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CustomerDTO
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Nazwa { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Properties.Resources))]
        public string Email { get; set; }
        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Numer { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Properties.Resources))]
        public string Adres { get; set; }
        public string MiastoKod { get; set; }
    }
}
