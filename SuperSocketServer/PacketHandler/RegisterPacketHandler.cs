using SuperSocket.SocketBase;
using SuperSocketServer.Packet;
using SuperSocketServer.PacketHandler;
using System;
using System.Collections.Generic;

namespace SuperSocketServer.Network.TCP
{
    // 패킷이 많아지면 Server.cs의 가독성이 떨어질 수 있기에 패킷 등록 부분만 따로 분리
    public partial class MyTcpServer : AppServer<MyTcpSession, MyBinaryRequestInfo>
    {
        Dictionary<int, Action<MyTcpSession, MyBinaryRequestInfo>> __handlerMap = new Dictionary<int, Action<MyTcpSession, MyBinaryRequestInfo>>();
        CommonHandler __commonHanlder = new CommonHandler();

        void RegistHandler()
        {
            // TODO : 패킷 추가되면 각 핸들러에 맞는 함수에 추가해 나갈 것
            CommonHandler();
        }

        void CommonHandler()
        {
            __handlerMap.Add((int)PacketID.PacketID_DummyChatReq, __commonHanlder.RequestDummyChat);
        }
    }
}
