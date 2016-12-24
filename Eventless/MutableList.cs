using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Eventless
{
    /// <summary>
    /// If your view model includes a plain <see cref="ObservableCollection{T}"/> of model 
    /// objects, and also a <see cref="Computed{T}"/> that reads the collection's items,
    /// it will not automatically update when you modify the collection itself. To make 
    /// this work, use <c>MutableList</c> instead. It has a <see cref="Value"/>
    /// property providing access to the collection itself, and it notifies that its
    /// own value has changed with the list is modified.
    /// </summary>
    /// <typeparam name="T">The list item type</typeparam>
    public sealed class MutableList<T> : IImmutable<ObservableCollection<T>>, IEquate<T>
    {
        private readonly ObservableCollection<T> _list = new ObservableCollection<T>();
        private readonly HashSet<int> _updatingIndices = new HashSet<int>();
        
        /// <summary>
        /// Constructs a <see cref="MutableList{T}"/>, optionally with the specified contents.
        /// </summary>
        /// <param name="init">The initial items in the list</param>
        /// <exception cref="RecursiveModificationException"></exception>
        public MutableList(IEnumerable<T> init = null)
        {                       
            if (init != null)
                _list.AddRange(init);

            _list.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    var count = Math.Min(e.OldItems.Count, e.NewItems.Count);
                    for (var i = 0; i < count; i++)
                    {
                        if (EqualityComparer((T)e.OldItems[i], (T)e.NewItems[i]))
                            continue;

                        if (!_updatingIndices.Add(i + e.OldStartingIndex))
                        {
                            throw new RecursiveModificationException();
                        }

                        try
                        {
                            PropertyChanged?.Invoke(this, Mutable.EventArgs);
                        }
                        finally
                        {
                            _updatingIndices.Remove(i + e.OldStartingIndex);
                        }
                        break;
                    }
                }
                else
                    PropertyChanged?.Invoke(this, Mutable.EventArgs);
            };
        }

        /// <summary>
        /// The collection, providing access to the items it stores. Modifications to
        /// the collections's contents count as a change to the value of this object.
        /// </summary>
        public ObservableCollection<T> Value
        {
            get
            {
                Computed.Listeners.Notify(this);
                return _list;
            }
        }

        /// <summary>
        /// See <see cref="INotifyPropertyChanged.PropertyChanged"/>. Raised whenever
        /// the list contents are modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// See <see cref="IEquate{T}.EqualityComparer"/>. Used to compare items in the list 
        /// to discover if they have actually been modified, to avoid raising notifications
        /// when the existing value is reassigned to a position in the list.
        /// </summary>
        public Func<T, T, bool> EqualityComparer { get; set; } = Mutable<T>.DefaultEqualityComparer;
    }
}