using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool In<T>(this T item, params T[] valuesToCheck)
        {
            return valuesToCheck.Contains(item);
        }

        public static (string login, string password) ToCredentials(this string[] arr)
        {
            return (arr[0], arr[1]);
        }
    }
}
