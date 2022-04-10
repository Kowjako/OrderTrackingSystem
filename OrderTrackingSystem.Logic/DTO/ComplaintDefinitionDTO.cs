using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public class ComplaintDefinitionDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }
        [Display(Name = "RemainDays", ResourceType = typeof(Properties.Resources))]
        public string RemainDays { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Properties.Resources))]
        public string Definition { get; set; }
    }

    public class ComplaintFolderDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }
        [Browsable(false)]
        public List<ComplaintFolderDTO> Children { get; set; }
    }
}
