using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketStudy.Network;
using System;

namespace SuperSocketStudy.Network
{
    public class MySession : AppSession<MySession, MyBinaryRequestInfo>
    {
        // 서버에 연결되는 Socket 로직 클래스
        // 해당 클래스로 클라이언트와의 연결, 해제, 데이터 입출력을 한다
    }
}
