using System;
using System.Collections.Generic;
using System.Threading;

namespace Eventless
{
    /* Intended for use as a singleton, to allow broadcasting events to the 
     * listener at the top of the stack, per-thread */
    public class ListenerStack<T>
    {
        private readonly ThreadLocal<Stack<Action<T>>> Stack =
                     new ThreadLocal<Stack<Action<T>>>(() => new Stack<Action<T>>());

        public void Push(Action<T> listener)
        {
            Stack.Value.Push(listener);
        }

        public void Pop()
        {
            Stack.Value.Pop();
        }

        public void Notify(T obs)
        {
            if (Stack.Value.Count != 0)
                Stack.Value.Peek()(obs);
        }
    }
}