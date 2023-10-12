using System;
using System.Collections.Concurrent;
using D2Core.infrastructure.events;

namespace D2Core.core.events
{
	public abstract class AbstractEventBus<T> : IEventBus<T>, IDisposable where T : IEvent
	{
        private bool _disposed;
        private ConcurrentDictionary<Type, IEventHandler<T>> subscriptions;

		public AbstractEventBus()
		{
            subscriptions = new();
		}

        ~AbstractEventBus() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Publish(T @event)
        {
            if (subscriptions.TryGetValue(@event.GetType(), out var handler))
                handler.handle(@event);
        }

        public void Subscribe(IEventHandler<T> eventHanlder, Type @event)
        {
            subscriptions.TryAdd(@event, eventHanlder);
            subscriptions[@event] = eventHanlder;
        }


        public void Unsubscribe(IEventHandler<T> eventHandler, Type @event)
        {
            subscriptions.TryRemove(new KeyValuePair<Type, IEventHandler<T>>(@event, eventHandler));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.subscriptions = new();
                }
            }
        }

    }
}

