using System;
using System.Collections.Generic;
using System.Threading;

namespace Eventless
{
    /* Intended for use as a singleton, to allow broadcasting events to the 
     * listener at the top of the stack, per-thread */
    public class ListenerStack<T>
    {
        private readonly ThreadLocal<Stack<Action<T>>> _stack =
                     new ThreadLocal<Stack<Action<T>>>(() => new Stack<Action<T>>());

        public void Push(Action<T> listener)
        {
            _stack.Value.Push(listener);
        }

        public void Pop()
        {
            _stack.Value.Pop();
        }

        public void Notify(T obs)
        {
            if (_stack.Value.Count != 0)
                _stack.Value.Peek()(obs);
        }
    }
}