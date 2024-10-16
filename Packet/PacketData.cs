using MessagePack;
using System;
using System.Text;

namespace SuperSocketShared.Packet
{
    public class SocketPacket
    {
        /// <summary>
        /// 패킷 길이 크기
        /// </summary>
        public static readonly int PACKET_LENGTH_SIZE = sizeof(int);
        /// <summary>
        /// 패킷 타입 크기
        /// </summary>
        public static readonly int PACKET_TYPE_SIZE = sizeof(int);
        /// <summary>
        /// 인코딩
        /// </summary>
        public static readonly Encoding DATA_ENCODING = Encoding.UTF8;

        /// <summary>
        /// PacketID, 패킷 타입
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 패킷 데이터
        /// </summary>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// 전체 패킷 크기
        /// </summary>
        public int Length
        {
            get
            {
                return PACKET_LENGTH_SIZE + PACKET_TYPE_SIZE + Data.Length;
            }
        }

        public int DataLength
        {
            get
            {
                return DATA_ENCODING.GetByteCount(Data);
            }
        }

        public byte[] DataBytes
        {
            get
            {
                return DATA_ENCODING.GetBytes(Data);
            }
        }

        public SocketPacket() { }
        public SocketPacket(int type)
        {
            Type = type;
            Data = string.Empty;
        }
        public SocketPacket(int type, string data)
        {
            Type = type;
            Data = data;
        }
        public SocketPacket(byte[] bytes)
        {
            Type = DecodeIntFromBytes(bytes);
            Data = DecodeStringFromBytes(bytes, 4);
        }

        public byte[] GetBytes()
        {
            byte[] packetBytes = new byte[Length];

            EncodeIntToBytes(Length - PACKET_LENGTH_SIZE, packetBytes);
            EncodeIntToBytes(Type, packetBytes, PACKET_LENGTH_SIZE);

            Buffer.BlockCopy(DataBytes, 0, packetBytes, PACKET_LENGTH_SIZE + PACKET_TYPE_SIZE, DataBytes.Length);

            return packetBytes;
        }

        public static int DecodeIntFromBytes(byte[] bytes, int offset = 0)
        {
            return BitConverter.ToInt32(bytes, offset);
        }

        public static string DecodeStringFromBytes(byte[] bytes, int offset = 0)
        {
            return DATA_ENCODING.GetString(bytes, offset, bytes.Length - offset);
        }

        public static void EncodeIntToBytes(int value, byte[] buffer, int offset = 0)
        {
            buffer[offset] = BitConverter.GetBytes(value)[0];
            buffer[offset + 1] = BitConverter.GetBytes(value)[1];
            buffer[offset + 2] = BitConverter.GetBytes(value)[2];
            buffer[offset + 3] = BitConverter.GetBytes(value)[3];
        }
    }

    [MessagePackObject]
    public class PKLoginReq
    {
        [Key(0)]
        public string UserName { get; set; } = string.Empty;
    }

    [MessagePackObject]
    public class PKLoginAck
    {
        [Key(0)]
        public ErrorEvent ErrorEvent { get; set; } = ErrorEvent.None;

        [Key(1)]
        public Int64 UID { get; set; } = 0;
    }

    [MessagePackObject]
    public class PKLoadCompletedReq
    {
    }

    [MessagePackObject]
    public class PKChatReq
    {
        [Key(0)]
        public string Sender { get; set; } = string.Empty;
        [Key(1)]
        public string Message { get; set; } = string.Empty;
    }

    [MessagePackObject]
    public class PKBroadcastChatAck
    {
        [Key(0)]
        public string Sender { get; set; } = string.Empty;
        [Key(1)]
        public string Message { get; set; } = string.Empty;
    }
}
