using System;
using D2Core.core.events.abstractions;
using D2Core.Game.network.eventHandlers;
using D2Core.infrastructure.events;

namespace D2Core.core.network.events
{
	public class ChatMessageServerEvent : ProtocolEvent
	{
		public ChatMessageServerEvent(ProtocolEventArgs eventArgs): base(eventArgs)
		{
		}
	}
}

