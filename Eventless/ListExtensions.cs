using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eventless
{
    /// <summary>
    /// Some helpful methods that are present in <see cref="List{T}"/> but annoyingly
    /// are not in <see cref="IList{T}"/>. By adding them as extensions they also
    /// become available in <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the specified set of objects from the list.
        /// </summary>
        /// <param name="list">The list to remove from</param>
        /// <param name="remove">The set of items to remove</param>
        /// <typeparam name="T">The item type</typeparam>
        public static void RemoveAll<T>(this IList<T> list, IEnumerable<T> remove)
        {
            foreach (var r in remove)
                list.Remove(r);
        }

        /// <summary>
        /// Adds the specified set of objects to the list.
        /// </summary>
        /// <param name="list">The list to add to</param>
        /// <param name="add">The set of items to add</param>
        /// <typeparam name="T">The item type</typeparam>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> add)
        {
            foreach (var r in add)
                list.Add(r);
        }
    }
}