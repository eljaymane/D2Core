using System;
using System.Buffers.Binary;
using System.Text;
using D2Core.core.exceptions;
using D2Core.infrastructure;

namespace D2Core.core.network.messages.chat
{
	public class ChatAbstractServerMessage : NetworkMessage
	{
        private bool _disposed;
        private bool _isInitialized = false;
      
        public uint channel = 0;
      
        public string content { get; private set; }
      
        public uint timestamp = 0;
      
        public string fingerprint = "";

        ~ChatAbstractServerMessage() => Dispose(false);

		public ChatAbstractServerMessage() : base(5870)
		{

		}

        public override uint GetMessageId()
        {
            return 5870;
        }

        public override void Reset()
        {
            this.channel = 0;
            this.content = "";
            this.timestamp = 0;
            this.fingerprint = "";
        }

        public void Initialize(uint channel, uint timestamp, string content="",string fingerprint = "")
        {
            this.channel = channel;
            this.content = content;
            this.timestamp = timestamp;
            this.fingerprint = fingerprint;
        }

        public override NetworkMessage Deserialize(ref MemoryStream buffer)
        {
            

            uint channel = (uint)buffer.ReadByte();
            var content = ASBinaryOps.readUTF(ref buffer);
            var timestamp = ASBinaryOps.readInt(ref buffer);
            var fingerprint = ASBinaryOps.readUTF(ref buffer);

            var msg = new ChatAbstractServerMessage();
                msg.Initialize(channel, (uint)timestamp, new string(content),fingerprint);

                return msg;
        }

        public override byte[] Pack(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(512);
            Serialize(ref ms);
            writePacket(buffer, this.ProtocolId, ms.GetBuffer()); ;
            return buffer;

        }

        public override void Serialize(ref MemoryStream buffer)
        {
            buffer.WriteByte((byte)channel);
            ASBinaryOps.writeUTF(ref buffer,content);
            if (this.timestamp < 0) throw new TimestampForbiddenValueException();
            ASBinaryOps.writeInt(ref buffer,timestamp);
            ASBinaryOps.writeUTF(ref buffer, fingerprint);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.Reset();
                }
            }
            base.Dispose(disposing);    
        }
    }
}

