using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketStudy.Network
{
    public class MyBinaryRequestInfo : BinaryRequestInfo
    {
        public int Head { get; private set; }

        public MyBinaryRequestInfo(int key, byte[] body)
            : base(null, body)
        {
            Head = key;
        }
    }

    class ReceiveFilter : FixedHeaderReceiveFilter<MyBinaryRequestInfo>
    {
        public ReceiveFilter() : base(8) { }/*Header 4Byte + Length 4Byte로 헤더 전체는 8Byte*/

        protected override int GetBodyLengthFromHeader(byte[] header, int offset/*헤더의 시작 지점*/, int length)
        {
            if (BitConverter.IsLittleEndian == false)
            {
                Array.Reverse(header, offset + 4, 4); // 헤더의 Length부분만 리틀 엔디안이면 빅 엔디안으로 변환
            }

            return BitConverter.ToInt32(header, offset + 4); // 헤더의 Length의 시작 메모리지점부터 32bit의 값만 가져온다 // Length값을 가져온다
        }

        protected override MyBinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            if (BitConverter.IsLittleEndian == false)
            {
                Array.Reverse(header.Array, 0, 4);
            }

            return new MyBinaryRequestInfo(BitConverter.ToInt32(header.Array, 0), bodyBuffer.CloneRange(offset, length));
        }
    }
}
