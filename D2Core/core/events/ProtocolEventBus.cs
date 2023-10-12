using System;
using D2Core.core.events;
using D2Core.core.events.abstractions;
using D2Core.core.network.eventHandlers;
using D2Core.core.network.events;
using Microsoft.Extensions.Logging;

namespace D2Core.core.network.events
{
	public class ProtocolEventBus : AbstractEventBus<ProtocolEvent>
	{
		private ILogger<ProtocolEventBus> _logger;

		public ProtocolEventBus(ILogger<ProtocolEventBus> logger)
		{
			this._logger = logger;
			this.Initialize();
		}

		public void Initialize()
		{
			this.Subscribe(new ChatMessageServerEventHandler(), typeof(ChatMessageServerEvent));
		}

    }
}

