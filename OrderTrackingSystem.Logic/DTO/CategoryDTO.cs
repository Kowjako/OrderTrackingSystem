using System.Collections.Generic;
using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public class CategoryDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Title { get; set; }
        [Browsable(false)]
        public List<CategoryDTO> Children { get; set; }
    }
}
