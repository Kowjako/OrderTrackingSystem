using OrderTrackingSystem.Logic.HelperClasses;
using System.Collections.Generic;
using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public class CategoryDTO : IComposite<CategoryDTO>
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Browsable(false)]
        public List<CategoryDTO> Children { get; set; }
        [Browsable(false)]
        public int? ParentId { get; set; }
    }
}
