using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketServer.Network.TCP
{
    class ReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public ReceiveFilter()
            : base(SocketPacket.PACKET_LENGTH_SIZE)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] bytes, int offset, int length)
        {
            //if (BitConverter.IsLittleEndian == false)
            //{
            //    Array.Reverse(bytes, offset + 4, 4);
            //}

            return BitConverter.ToInt32(bytes, offset);
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> bytes, byte[] bodyBuffer, int offset, int length)
        {
            //if (BitConverter.IsLittleEndian == false)
            //{
            //    Array.Reverse(bytes.Array, 0, 4);
            //}

            return new BinaryRequestInfo(null, bodyBuffer.CloneRange(offset, length));
        }
    }
}
