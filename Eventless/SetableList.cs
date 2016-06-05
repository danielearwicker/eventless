using System;
using System.Collections;
using System.Collections.Generic;

namespace Eventless
{
    public sealed class SetableList<T> : ISetableList<T>
    {
        private readonly List<T> _list;
        
        public SetableList(IEnumerable<T> init = null)
        {
            EqualityComparer = Setable<T>.DefaultEqualityComparer;

            _list = new List<T>();
            if (init != null)
                _list.AddRange(init);
        }

        public IList<T> Value
        {
            get
            {
                Computed.Listeners.Notify(this);
                return _list;
            }
        }

        public event Action Changed;

        public event Action<int> Added;
        public event Action<int> Updated;
        public event Action<int> Removed;
        public event Action Cleared;

        // Applies to items (you can't replace the whole list in one shot)
        public Func<T, T, bool> EqualityComparer { get; set; }

        private void Writing(Action<int> also = null, int index = -1)
        {
            var evt = Changed;
            if (evt != null)
                evt();

            if (also != null)
                also(index);
        }

        public int IndexOf(T item)
        {
            Computed.Listeners.Notify(this);
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            Writing(Added, index);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            Writing(Removed, index);
        }

        private Stack<int> _updatingIndexes;

        public T this[int index]
        {
            get
            {
                Computed.Listeners.Notify(this);
                return _list[index];
            }
            set
            {
                if (EqualityComparer(_list[index], value))
                    return;

                if (_updatingIndexes == null)
                    _updatingIndexes = new Stack<int>();

                if (_updatingIndexes.Contains(index))
                    throw new RecursiveModificationException();

                _list[index] = value;
                _updatingIndexes.Push(index);

                try
                {
                    Writing(Updated, index);
                }
                finally
                {
                    _updatingIndexes.Pop();
                }
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
            Writing(Added, _list.Count - 1);
        }

        public void Clear()
        {
            _list.Clear();
            Writing();
            var clr = Cleared;
            if (clr != null)
                clr();
        }

        public bool Contains(T item)
        {
            Computed.Listeners.Notify(this);
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Computed.Listeners.Notify(this);
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                Computed.Listeners.Notify(this);
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            Computed.Listeners.Notify(this);
            var index = _list.IndexOf(item);
            if (index == -1)
                return false;

            RemoveAt(index);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Computed.Listeners.Notify(this);
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}