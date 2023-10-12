using System;
using D2Core.infrastructure.events;

namespace D2Core.core.events
{
	public interface IEventHandler<T> where T : IEvent
	{
		public void handle(IEvent @event);
	}
}

