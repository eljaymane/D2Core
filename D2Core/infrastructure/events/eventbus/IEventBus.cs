using System;
using D2Core.core.events;
using D2Core.core.events.abstractions;
using D2Core.core.events.eventbus;
using D2Core.infrastructure.events;

namespace D2Core.core
{
	public interface IEventBus<T> where T : IEvent
	{
		void Publish(T @event);

		void Subscribe(IEventHandler<T> eventHanlder,Type @eventType);

		void Unsubscribe(IEventHandler<T> eventHandler, Type @eventType);
	}
}

