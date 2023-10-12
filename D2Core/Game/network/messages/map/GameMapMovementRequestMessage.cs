using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D2Core.infrastructure;

namespace D2Core.core.network.messages
{
    public class GameMapMovementRequestMessage : NetworkMessage
    {
        private bool _disposed;

        public static uint protocolId = 1119;
       
        private bool _isInitialized = false;
      
        public IEnumerable<uint> keyMovements;
      
        public double mapId = 0;

        //private var _keyMovementstree:FuncTree;

        public GameMapMovementRequestMessage(): base(protocolId)
        {
            this.keyMovements = new List<uint>();
        }

        public GameMapMovementRequestMessage(IEnumerable<uint> keyMovements, double mapId) : base(protocolId)
        {
            this.keyMovements = keyMovements;
            this.mapId = mapId;
        }

        public override bool GetIsInitialized()
        {
            return this.GetIsInitialized();
        }

        public override uint GetMessageId()
        {
            return base.GetMessageId();
        }

        public void Initialize(IEnumerable<uint> keyMovements=null, double mapId = 0)
        {
            this.keyMovements = keyMovements;
            this.mapId = mapId;
        }

        public override void Reset()
        {
            this.keyMovements = new List<uint>();
            this.mapId = 0;
            this._isInitialized = false;
        }

        public override void Serialize(ref MemoryStream ms)
        {
            for(int i = 0; i<this.keyMovements.Count(); i++)
            {
                if (this.keyMovements.ElementAt(i) < 0) throw new Exception("Forbidden value of keyMovements");
                ASBinaryOps.writeShort(ref ms, (ushort)this.keyMovements.ElementAt(i));
            }

            if (this.mapId < 0 || this.mapId > 9007199254740992) throw new Exception("Forbidden value for map id");
            ASBinaryOps.writeDouble(ref ms, this.mapId);
        }

        public override NetworkMessage Deserialize(ref MemoryStream buffer)
        {
            uint value = 0;
            uint keyMovementslen = (uint)ASBinaryOps.readShort(ref buffer);
            for(uint i = 0; i < keyMovementslen; i++)
            {
                value = (uint)ASBinaryOps.readShort(ref buffer);
                if(value < 0)
                {
                    throw new Exception("Forbidden value on elements of keyMovements");
                }
                this.keyMovements.Append(value);
            }
            this.mapId = ASBinaryOps.readDouble(ref buffer);
            return new GameMapMovementRequestMessage(this.keyMovements, this.mapId);
        }


        public override byte[] Pack(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(new byte[512]);
            this.Serialize(ref ms);
            //IF HASH
            //HASH(DATA)
            writePacket(buffer, this.GetMessageId(), ms.GetBuffer());
            return buffer;
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