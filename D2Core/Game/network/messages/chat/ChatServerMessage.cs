using System;
using D2Core.core.network.messages.chat;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System.Threading.Channels;
using D2Core.core.exceptions;
using System.Buffers.Binary;
using System.Text;
using D2Core.infrastructure;
using D2Core.core.network.events;
using D2Core.Game.network.eventHandlers;

namespace D2Core.core.network.messages
{
	public class ChatServerMessage : ChatAbstractServerMessage
	{
        private bool _disposed;

        private uint protocolId = 8812;

        private bool _isInitialized = false;
      
        public double senderId = 0;
      
        public string senderName = "";
      
        public string prefix = "";
      
        public uint senderAccountId = 0;

        ~ChatServerMessage() => Dispose(false);

	    public ChatServerMessage():base()
		{
		}

        public override bool GetIsInitialized() 
        {
             return base.GetIsInitialized() && this._isInitialized;
        }

        public override uint GetMessageId()
        {
            return 8812;
        }

        public void Initialize(uint channel = 0,string content = "", uint timestamp = 0, string fingerprint = "", double senderId = 0, string senderName = "", string prefix = "", uint senderAccountId = 0)
        {
            base.Initialize(channel, timestamp, content, fingerprint);
            this.senderId = senderId;
            this.senderName = senderName;
            this.prefix = prefix;
            this.senderAccountId = senderAccountId;
            this._isInitialized = true;
      }
      
      public override void Reset() 
      {
            base.Reset();
            this.senderId = 0;
            this.senderName = "";
            this.prefix = "";
            this.senderAccountId = 0;
            this._isInitialized = false;
      }
      
      public override byte[] Pack(byte[] buffer)
      {
            MemoryStream ms = new MemoryStream(512);
            Serialize(ref ms);
            writePacket(buffer, GetMessageId(), ms.GetBuffer());
            return buffer;
      }

      public override void Serialize(ref MemoryStream buffer) 
      {
            base.Serialize(ref buffer);
            if (this.senderId < -9007199254740992 || this.senderId > 9007199254740992)
            {
                throw new SenderIdException();
            }
           
            ASBinaryOps.writeDouble(ref buffer,senderId);//Should be converted to BEndian
            ASBinaryOps.writeUTF(ref buffer,this.senderName);
            ASBinaryOps.writeUTF(ref buffer,this.prefix);

            if (this.senderAccountId < 0)
            {
                throw new Exception("Forbidden value (" + this.senderAccountId + ") on element senderAccountId.");
            }
            ASBinaryOps.writeInt(ref buffer,this.senderAccountId);
      }
      
      public override ChatServerMessage Deserialize(ref MemoryStream data) 
      {
            ChatAbstractServerMessage absMsg;

            absMsg = (ChatAbstractServerMessage)base.Deserialize(ref data);
            var senderId = ASBinaryOps.readDouble(ref data);

            data.Seek(10,SeekOrigin.Current);//Skipping the some weird space..
           
            var senderName = ASBinaryOps.readUTF(ref data);
            var prefix = ASBinaryOps.readUTF(ref data);
            var senderAccountId = ASBinaryOps.readInt(ref data);
                
            var msg = new ChatServerMessage();
            msg.Initialize(absMsg.channel, absMsg.content, absMsg.timestamp, absMsg.fingerprint,
                senderId, senderName.ToString(), prefix, senderAccountId);

            //Console.WriteLine($"[{senderName}] : {msg.content}");
                return msg;
           
      }
        public override void RaiseEvent(ProtocolEventBus eventBus)
        {
            eventBus.Publish(new ChatMessageServerEvent(new ProtocolEventArgs(this,DateTime.UtcNow)));
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

