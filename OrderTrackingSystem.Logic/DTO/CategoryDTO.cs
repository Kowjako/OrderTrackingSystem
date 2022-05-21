using OrderTrackingSystem.Logic.HelperClasses;
using System.Collections.Generic;
using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class CategoryDTO : IComposite<CategoryDTO>
    {
        public string Name { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int? ParentId { get; set; }

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public List<CategoryDTO> Children { get; set; }

        #endregion


    }
}
