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

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(var item in items)
            {
                action(item);
            }
        }

        public static bool In<T>(this T item, params T[] valuesToCheck)
        {
            return valuesToCheck.Contains(item);
        }

        public static (string login, string password) ToCredentials(this string[] arr)
        {
            if (arr.Length != 2) throw new InvalidOperationException("Wyamagana ilość parametrów 2 - login + hasło");
            return (arr[0], arr[1]);
        }
    }
}
