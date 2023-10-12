using System;
using System.Buffers.Binary;

namespace D2Core.core.network.messages.basic
{
	public class BasicPingMessage : NetworkMessage
	{
        private bool _disposed;
        private bool _isInitialized = false;
        public bool quiet = false;

        ~BasicPingMessage() => Dispose(false);

        public BasicPingMessage() : base(6075)
		{
		}

        public BasicPingMessage(bool isInitialized,bool quiet) : base(6075)
        {
            this._isInitialized = isInitialized;
            this.quiet = quiet;
        }



        public override bool GetIsInitialized()
        {
         return this._isInitialized;
        }


        public BasicPingMessage initBasicPingMessage(bool quiet = false)
        {
            this.quiet = quiet;
            this._isInitialized = true;
            return this;
        }
      
       public void reset()
       {
            this.quiet = false;
            this._isInitialized = false;
       }
      
       public override byte[] Pack(byte[] buffer) 
       {
            MemoryStream ms = new MemoryStream(new byte[512]);
            this.Serialize(ref ms);
            writePacket(buffer, this.GetMessageId(),ms.GetBuffer());
            return buffer;
       }
     
      public override void Serialize(ref MemoryStream buffer)
      {
            var bWriter = new BinaryWriter(buffer);
                bWriter.Write(this.quiet);
      }

      public override BasicPingMessage Deserialize(ref MemoryStream buffer)
        {
            using(var bReader = new BinaryReader(buffer))
            {
                this.quiet = bReader.ReadBoolean();
                bReader.Close();
            }
            return this;
        }

        public override void Reset()
        {
            this.quiet = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.Reset();
                }
                base.Dispose(true);
            }
        }
    }
}

