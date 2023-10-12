using System;
using D2Core.infrastructure.events;
using D2Core.core.network;
using D2Core.Game.network.eventHandlers;

namespace D2Core.core.network.events 
{
	public abstract class ProtocolEvent : IEvent
	{
		public ProtocolEventArgs eventArgs { get; }

		public ProtocolEvent(ProtocolEventArgs eventArgs)
		{
			this.eventArgs = eventArgs;
		}

        public ProtocolEventArgs GetEventArgs()
        {
			return eventArgs;
        }
    }
}

