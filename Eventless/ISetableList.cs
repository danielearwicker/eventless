using System;
using System.Collections.Generic;

namespace Eventless
{
    public interface ISetableList<T> : IGetable<IList<T>>, IList<T>
    {
        event Action<int> Added;
        event Action<int> Updated;
        event Action<int> Removed;
        event Action Cleared;
    }

    public static class SetableListExtensions
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