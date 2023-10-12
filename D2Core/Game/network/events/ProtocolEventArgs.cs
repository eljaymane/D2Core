using System;
using D2Core.core.network;

namespace D2Core.Game.network.eventHandlers
{
	public class ProtocolEventArgs : EventArgs
	{
		public NetworkMessage? message;
        public DateTime createdAt { get; }

		public ProtocolEventArgs(NetworkMessage message,DateTime createdAt)
		{
			this.message = message;
			this.createdAt = createdAt;
		}
    }
}

