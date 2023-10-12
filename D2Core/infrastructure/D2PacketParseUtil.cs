using System;
namespace D2Core.infrastructure
{
	public static class D2PacketParseUtil
	{
        public static uint getProtocolId(ushort header)
        {
            return (uint)(header >> 2);
        }

        public static uint getLenType(ushort header)
        {
            return (uint)(header & 3);
        }

        public static int getLength(uint lenType, byte[] packet)
        {
            MemoryStream ms = new MemoryStream(packet.Skip(2).ToArray());//Skiping the header
            uint length = 0;
            while (lenType-- > 0)
            {
                uint d = 0;
                d = (uint)ms.ReadByte();
                length = (length << 8) + d;
            }
            ms.Close();
            return (int)length;
        }

        public static byte[] getData(byte[] packet, uint lenType, int length)
        {
            var data = packet.Skip(2).Skip((int)lenType).Skip(1).ToArray();
            if (data.Length < length) return new byte[10];//Skiping header + data length (byte * lentype) + instanceId
            MemoryStream ms = new MemoryStream(data, 0, length);
            BinaryReader bReader = new BinaryReader(ms);
            byte[] b = bReader.ReadBytes(length);
            ms.Close();
            return b;
        }
    }
}

