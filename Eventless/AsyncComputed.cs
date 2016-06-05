using System;
using System.Threading.Tasks;

namespace Eventless
{
    public class AsyncComputed<T> : Forwarder<Setable<T>, T>, IGetable<T>, ICanThrottle<AsyncComputed<T>>
    {
        private readonly Computed<Computed.Void> _computed;
        private readonly Func<Task<T>> _compute;
        private int _invocationCounter;
        
        public AsyncComputed(Func<Task<T>> asyncObs)
            : base(new Setable<T>())
        {
            _compute = asyncObs;
            _computed = Computed.Do(Recalculate);
        }

        private async void Recalculate()
        {
            var invocation = ++_invocationCounter;
            var task = _compute();
            if (task == null)
                Impl.Value = default(T);
            else
            {
                var result = await task;
                if (invocation == _invocationCounter)
                    Impl.Value = result;
            }
        }

        public T Value
        {
            get { return Impl.Value; }
        }

        public AsyncComputed<T> SetThrottler(Func<Action, Action> throttler)
        {
            _computed.SetThrottler(throttler);
            return this;
        }
    }
}