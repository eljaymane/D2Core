using System;
using D2Core.core.events;
using D2Core.core.network.events;

namespace D2Core.Game.network.events.eventHandlers
{
	public interface IProtocolEventHandler : IEventHandler<ProtocolEvent>
	{
		public void handle(ProtocolEvent @event);
	}
}

