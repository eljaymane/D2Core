using System;
using System.Xml.Serialization;
using D2Core.core.data;
using D2Core.core.exceptions;
using D2Core.core.network;
using D2Core.core.network.events;
using D2Core.infrastructure.data;
using Microsoft.Extensions.Logging;

namespace D2Core.core
{
	public class MessageFactory
	{
		private IList<MessageDataEntry> messageStore;
		private ILogger<MessageFactory> logger;
		private ProtocolEventBus eventBus;

		public MessageFactory(ILogger<MessageFactory> logger,ProtocolEventBus eventBus)
		{
			this.logger = logger;
			messageStore = new List<MessageDataEntry>();
			this.eventBus = eventBus;
			readXmlData("/Volumes/Demesure/Dinvoker/scripts_cs/networkMessages.xml");
		}

		public void readXmlData(string path)
		{
			var file = new FileInfo(path);
			if (!file.Exists) throw new XmlStoreFileNotFoundException();
			MessageDataEntries entries = new MessageDataEntries();
			XmlSerializer xml = new XmlSerializer(entries.GetType());
			using(var reader = new StreamReader(path))
			{
				messageStore = ((MessageDataEntries)(xml.Deserialize(reader))).messages;
			}
			logger.LogInformation($"Added {messageStore.Count()} NetworkMessage to the store !");

		}

		public string getClassNameById(uint protocoId)
		{
			var msg = messageStore.Where(msg => msg.ProtocolId == protocoId).FirstOrDefault();
			return msg != null ? msg.ClassName : "";
		}

		public Task BuildMessage(uint protocolId, byte[]? data)
		{
			var clazz = messageStore.Where(msg => msg.ProtocolId == protocolId).FirstOrDefault();
			if(clazz == null) { logger.LogInformation($"No entry found for : {protocolId}"); return null; }
			else
			{
                try
                {
                    dynamic message = Activator.CreateInstance(null, "D2Core.core.network.messages." + clazz?.ClassName).Unwrap();
                    message.ProtocolId = protocolId;
                    var d = new MemoryStream(data);
                    var m = message.Deserialize(ref d);
                    if (m != null) m.RaiseEvent(this.eventBus);

                    return data != null ? m : message;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
					

                }

                return Task.CompletedTask;
            }
			
			
			
					
			
            
        }
	}
}

