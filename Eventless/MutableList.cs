using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Eventless
{
    public sealed class MutableList<T> : IImmutable<ObservableCollection<T>>, IEquate<T>
    {
        private readonly ObservableCollection<T> _list = new ObservableCollection<T>();
        private readonly HashSet<int> _updatingIndices = new HashSet<int>();
        
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

        public ObservableCollection<T> Value
        {
            get
            {
                Computed.Listeners.Notify(this);
                return _list;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Func<T, T, bool> EqualityComparer { get; set; } = DefaultEqualityComparer;

        public static bool DefaultEqualityComparer(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }
    }
}