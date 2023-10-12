using System;
using D2Core.core.events;
using D2Core.core.network.events;
using D2Core.Game.network.events.eventHandlers;
using D2Core.infrastructure.events;

namespace D2Core.core.network.eventHandlers
{
	public abstract class ProtocolEventHandler : IProtocolEventHandler
	{
		
        public ProtocolEventHandler()
		{
		}

		public abstract void handle(ProtocolEvent @event);

        public void handle(IEvent @event)
        {
            handle((ProtocolEvent)@event);
        }
    }
}

