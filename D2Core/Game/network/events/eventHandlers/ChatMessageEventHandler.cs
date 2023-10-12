using System;
using D2Core.core.events;
using D2Core.core.network.events;
using D2Core.Game.network.eventHandlers;
using D2Core.infrastructure.events;
using Microsoft.Extensions.Logging;

namespace D2Core.core.network.eventHandlers
{
	public class ChatMessageServerEventHandler : ProtocolEventHandler
	{
        public static EventHandler<ProtocolEventArgs> ChatMessageEvent;
        public ChatMessageServerEventHandler()
		{
		}

        public override void handle(ProtocolEvent @event)
        {
			var eventArg = (ChatMessageServerEvent)@event;
			EventHandler<ProtocolEventArgs> handler = ChatMessageServerEventHandler.ChatMessageEvent;
			if(handler != null)
			{
				handler.Invoke(this, @event.eventArgs);
			}
        }
    }
}

