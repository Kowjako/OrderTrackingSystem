using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.HelperClasses
{
    public static class Extensions
    {
        public async static Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            foreach(var item in items)
            {
                await func(item);
            }
        }
    }
}
