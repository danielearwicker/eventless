using System;
using System.Collections.Generic;

namespace Eventless
{
    public interface IWriteableList<T> : IReadable<IList<T>>, IList<T>
    {
        event Action<int> Added;
        event Action<int> Updated;
        event Action<int> Removed;
        event Action Cleared;
    }

    public static class WriteableListExtensions
    {
        public static void RemoveAll<T>(this IList<T> list, IEnumerable<T> remove)
        {
            foreach (var r in remove)
                list.Remove(r);
        }
    }
}