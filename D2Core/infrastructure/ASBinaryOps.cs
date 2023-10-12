using System;
using System.Buffers.Binary;
using System.Text;

namespace D2Core.infrastructure
{
	public static class ASBinaryOps
	{
        //A bit tricky, but we need to first read a byte that contains the length. We should do the same when writing the packet. Source : https://help.adobe.com/fr_FR/FlashPlatform/reference/actionscript/3/flash/utils/ByteArray.html#readUTF()
        public static string readUTF(ref MemoryStream ms)
		{
                var bReader = new BinaryReader(ms);
                int strlen = bReader.ReadByte();
                Console.WriteLine($"Content size : {strlen} buffer : {bReader.BaseStream.Position} / {bReader.BaseStream.Length}");
                if (strlen <= 0) return "";
                return Encoding.UTF8.GetString(bReader.ReadBytes(strlen));
		}

        public static void writeUTF(ref MemoryStream ms,string str)
        {
                var bWriter = new BinaryWriter(ms);
                byte len = (byte)str.Length;
                bWriter.Write(len);
                if (str.Length == 0) return;
                bWriter.Write(str);
        }

        public static ushort readShort(ref MemoryStream ms)
        {
                var bReader = new BinaryReader(ms);
                ushort result = BinaryPrimitives.ReadUInt16BigEndian(bReader.ReadBytes(2));
                return result;
        }

        public static void writeShort(ref MemoryStream ms, ushort value)
        {
                var bWriter = new BinaryWriter(ms);
                if (BitConverter.IsLittleEndian) bWriter.Write(toBigEndian(value));
                else bWriter.Write(value);
        }


        public static uint readInt(ref MemoryStream ms)
        {
                var bReader = new BinaryReader(ms);
                uint result = BinaryPrimitives.ReadUInt32BigEndian(bReader.ReadBytes(sizeof(uint)));
                return result;

        }

        public static void writeInt(ref MemoryStream ms, uint value)
        {
                var bWriter = new BinaryWriter(ms);
                if (BitConverter.IsLittleEndian) bWriter.Write(toBigEndian(value));
                else bWriter.Write(value);

        }

        public static double readDouble(ref MemoryStream ms)
        {
                var bReader = new BinaryReader(ms);
                double result = BinaryPrimitives.ReadDoubleBigEndian(bReader.ReadBytes(8));
                return result;
        }

        public static void writeDouble(ref MemoryStream ms, double value)
        {
                var bWriter = new BinaryWriter(ms);
                if (BitConverter.IsLittleEndian) bWriter.Write(toBigEndian(value));
                else bWriter.Write(value);

        }

        public static byte[] toBigEndian(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public static byte[] toBEndian(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public static byte[] toBEndian(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, bytes.Length);
            }
            return bytes;
        }

    }
}

