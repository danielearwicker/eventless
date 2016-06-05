using System.Collections.Generic;

namespace Eventless
{
    public static class ListExtensions
    {
        public static void RemoveAll<T>(this IList<T> list, IEnumerable<T> remove)
        {
            foreach (var r in remove)
                list.Remove(r);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> add)
        {
            foreach (var r in add)
                list.Add(r);
        }
    }
}