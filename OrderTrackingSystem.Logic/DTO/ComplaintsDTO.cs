using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public class ComplaintsDTO
    {
        [Display(Name = "Order", ResourceType = typeof(Properties.Resources))]
        public string OrderNumber { get; set; }
        [Display(Name = "State", ResourceType = typeof(Properties.Resources))]
        public string State { get; set; }
        [Display(Name = "Date", ResourceType = typeof(Properties.Resources))]
        public string Date { get; set; }
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

    public class ComplaintDefinitionDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }
        [Display(Name = "RemainDays", ResourceType = typeof(Properties.Resources))]
        public byte RemainDays { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Properties.Resources))]
        public string Definition { get; set; }

        [Browsable(false)]
        public int ComplaintFolderId { get; set; }
    }
}
