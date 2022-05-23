using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO.Pagination
{
    public class Paginator
    {
        private const int PAGE_SIZE = 10;

        public static IEnumerable<T> GetPaginatedList<T>(IQueryable<T> list, int pageNumber) where T : class, IPagedEntity
        {
            return list.OrderBy(x => x.Id).Skip(PAGE_SIZE * pageNumber).Take(PAGE_SIZE).ToList().Select((x, idx) =>
            {
                x.RowNumber = idx++;
                return x;
            });
        }
    }
}
