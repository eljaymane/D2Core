using System.Buffers.Binary;
using D2Core.core.network.events;

namespace D2Core.core.network
{
	public abstract class NetworkMessage : IDisposable
	{
        private bool _disposed;
        public uint ProtocolId { get; set; }
        private static uint GLOBAL_INSTANCE_ID = 0;
      
        public static int BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;

        public static uint BIT_MASK = 3;

       //public static unsafe delegate*<void> HASH_FUNCTION;


        private uint _instance_id;
        public int receptionTime;
        public string sourceConnection;
        public bool _unpacked = false;

		public NetworkMessage(uint protocolId) : base()
		{
            this._instance_id = ++GLOBAL_INSTANCE_ID;
            this.ProtocolId = protocolId;
        }

        ~NetworkMessage() => Dispose(false);

        #region I/O
        public abstract void Serialize(ref MemoryStream buffer);
        public abstract NetworkMessage Deserialize(ref MemoryStream buffer);
        public abstract byte[] Pack(byte[] buffer);

        public void writePacket(byte[] buffer, uint id, byte[] data)
        {

            uint high = 0;
            uint low = 0;
            uint typeLen = computeTypeLen((uint)data.Length);

            using (var bWriter = new BinaryWriter(new MemoryStream(buffer)))
            {
                var header = subComputeStaticHeader(id, typeLen);
                bWriter.Write(BinaryPrimitives.ReverseEndianness(header));
                bWriter.Write(BinaryPrimitives.ReverseEndianness(this._instance_id));
                switch (typeLen)
                {
                    case 0:
                        bWriter.Close();
                        return;
                    case 1:
                        bWriter.Write(BinaryPrimitives.ReverseEndianness((byte)data.Length));
                        break;
                    case 2:
                        bWriter.Write(BinaryPrimitives.ReverseEndianness((short)data.Length));
                        break;
                    case 3:
                        high = (uint)(data.Length >> 16 & 255);
                        low = (uint)(data.Length & 65535);
                        bWriter.Write(BinaryPrimitives.ReverseEndianness((byte)high));
                        bWriter.Write(BinaryPrimitives.ReverseEndianness((short)low));
                        break;

                }
                bWriter.Write(data, 0, data.Length);//Assuming it's already Big Endian
                bWriter.Close();
            }
        }

        private static uint computeTypeLen(uint length)
        {
            if (length > 65535)
            {
                return 3;
            }
            if (length > 255)
            {
                return 2;
            }
            if (length > 0)
            {
                return 1;
            }
            return 0;
        }

        private static short subComputeStaticHeader(uint msgId, uint typeLen)
        {
            return (short)(msgId << BIT_RIGHT_SHIFT_LEN_PACKET_ID | typeLen);
        }
        #endregion

        public abstract void Reset();
        public virtual bool GetIsInitialized()
        {
            throw new Exception();
        }

        public virtual uint GetMessageId()
        {
            return this.ProtocolId > 0 ? this.ProtocolId : 0;
        }


        public bool GetUnpacked() 
            {
             return this._unpacked;
            }

        public void SetUnpacked(bool value) 
          {
             this._unpacked = value;
          }

        public virtual void RaiseEvent(ProtocolEventBus eventBus)
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }
                _disposed = true;
            }
        }
    }
}

