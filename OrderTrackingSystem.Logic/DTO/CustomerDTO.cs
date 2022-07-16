using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class CustomerDTO
    {
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Properties.Resources))]
        public string Email { get; set; }

        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Number { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Properties.Resources))]
        public string Address { get; set; }

        public string CityWithCode { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public decimal Balance { get; set; }

        #endregion
    }
}
