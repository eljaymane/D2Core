using System;
using D2Core.core.data;
using System.Xml.Serialization;

namespace D2Core.infrastructure.data
{

    [XmlRoot("NetworkMessages")]
    public class MessageDataEntries
    {
        [XmlArray("NetworkMessageArray")]
        [XmlArrayItem("NetworkMessages")]
        public List<MessageDataEntry> messages { get; set; }

        public MessageDataEntries()
        {

        }
    }
}

