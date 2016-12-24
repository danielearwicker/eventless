using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Eventless
{    
    /// <summary>
    /// This is the basis of the mechanism by which dependencies are automatically tracked.
    /// While a reaction is executing, it uses <see cref="Push"/> to place a listener at 
    /// the top of the stack. Every time an observable is read, it passes itself to 
    /// <see cref="Notify"/>. In this way, reactions can discover what observables they
    /// depend on, and so update their subscriptions accordingly.
    /// </summary>
    /// <typeparam name="T">Some type representing a source of events about an observable
    /// value, e.g. <see cref="INotifyPropertyChanged"/></typeparam>
    public class ListenerStack<T>
    {
        private readonly ThreadLocal<Stack<Action<T>>> _stack =
                     new ThreadLocal<Stack<Action<T>>>(() => new Stack<Action<T>>());

        /// <summary>
        /// Registers the specified listener at the top of the stack so it will receive
        /// read notifications.
        /// </summary>
        /// <param name="listener">An action that will be called every time an observable
        /// value is read</param>
        public void Push(Action<T> listener)
        {
            _stack.Value.Push(listener);
        }

        /// <summary>
        /// Unregisters the listener at the top of the stack.
        /// </summary>
        public void Pop()
        {
            _stack.Value.Pop();
        }

        /// <summary>
        /// Passes an observable value to the top-most listener, so it can maintain
        /// a subscription to that value's change events. 
        /// </summary>
        /// <param name="obs"></param>
        public void Notify(T obs)
        {
            if (_stack.Value.Count != 0)
                _stack.Value.Peek()(obs);
        }
    }
}