using System;
using D2Core.core.network;

namespace D2Core.core
{
	public class ResponseBuilder
	{
		private MessageFactory messageFactory;

		public ResponseBuilder()
		{

		}


		public NetworkMessage getResponse(NetworkMessage msg)
		{
			return msg;
		}
	}
}

