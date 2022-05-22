using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO.Pagination
{
    public interface IPagedEntity
    {
        int RowNumber { get; set; }
    }
}
