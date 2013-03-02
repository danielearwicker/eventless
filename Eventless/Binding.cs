using System;

namespace Eventless
{
    public static class Binding
    {
        public static readonly ListenerStack<Action> Log = new ListenerStack<Action>();

        public static void EmptyAction() { }

        public static Action CaptureUnbind(Action bindingActivity)
        {
            try
            {
                Action multicastUnbinders = null;
                Log.Push(nestedUnbinder => multicastUnbinders += nestedUnbinder);
                bindingActivity();
                return multicastUnbinders ?? EmptyAction;
            }
            finally
            {
                Log.Pop();
            }
        }
    }
}