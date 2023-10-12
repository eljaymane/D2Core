using System;
using D2Core.core.events.abstractions;
using D2Core.core.network.events;

namespace D2Core.core.events.eventbus
{
	public abstract class AbstractProtocolEventHandler
	{
		private IEventBus<ProtocolEvent> eventbus;

		public AbstractProtocolEventHandler()
		{
			
		}
	}
}

