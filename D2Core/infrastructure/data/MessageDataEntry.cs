using System;
using System.Xml.Serialization;

namespace D2Core.core.data
{

    [XmlType("NetworkMessage")] // define Type
    public class MessageDataEntry
    {
        [XmlAttribute]
        public uint ProtocolId { get; set; }
        [XmlAttribute]
        public string ClassName { get; set; }

        public MessageDataEntry(uint protocolId, string className)
        {
            this.ProtocolId = protocolId;
            this.ClassName = className;
        }

        public MessageDataEntry()
        {

        }
    }
}

