using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO.Pagination
{
    public class Paginator
    {
        public static IEnumerable<T> GetPaginatedList<T>(IEnumerable<T> list) where T : IPagedEntity
        {
            return list.Select((record, idx) =>
            {
                record.RowNumber = idx++;
                return record;
            });
        }
    }
}
