using OrderTrackingSystem.Logic.DTO.Pagination;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class ComplaintsDTO : IPagedEntity
    {
        [Display(Name = "Order", ResourceType = typeof(Properties.Resources))]
        public string OrderNumber { get; set; }

        [Display(Name = "State", ResourceType = typeof(Properties.Resources))]
        public string State { get; set; }

        [LongDateField]
        [Display(Name = "Date", ResourceType = typeof(Properties.Resources))]
        public DateTime Date { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public int StateId { get; set; }

        [Browsable(false)]
        public int OrderId { get; set; }

        [Browsable(false)]
        public DateTime? SolutionDate { get; set; }

        [Browsable(false)]
        public DateTime? EndDate { get; set; }

        [Browsable(false)]
        public List<DateTime?> ComplaintStateDates => new List<DateTime?> { Date, SolutionDate, EndDate };

        #endregion

        #region IPagedEntity implementation

        [Browsable(false)]
        public int RowNumber { get; set; }

        #endregion

    }

    #pragma warning disable CS1591
    public class ComplaintFolderDTO : IComposite<ComplaintFolderDTO>
    {
        
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public List<ComplaintFolderDTO> Children { get; set; }

        [Browsable(false)]
        public int? ParentId { get; set; }

        #endregion

    }

    #pragma warning disable CS1591
    public class ComplaintDefinitionDTO
    {       
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }

        [Display(Name = "RemainDays", ResourceType = typeof(Properties.Resources))]
        public byte RemainDays { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Properties.Resources))]
        public string Definition { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public int ComplaintFolderId { get; set; }

        #endregion


    }
}
