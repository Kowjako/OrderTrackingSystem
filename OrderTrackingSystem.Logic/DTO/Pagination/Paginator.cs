using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO.Pagination
{
    public class Paginator
    {
        public static IEnumerable<T> GetPaginatedList<T>(IQueryable<T> list) where T : class, IPagedEntity
        {
            return list.OrderBy(x => x.Id).Skip(0).Take(10).ToList();
        }
    }
}
