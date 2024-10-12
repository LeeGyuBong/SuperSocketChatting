using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketServer.PacketHandler;
using SuperSocketShared.Packet;
using System;
using System.Collections.Generic;

namespace SuperSocketServer.Network.TCP
{
    // 패킷이 많아지면 Server.cs의 가독성이 떨어질 수 있기에 패킷 등록 부분만 따로 분리
    public partial class MyTcpServer : AppServer<MyTcpSession, BinaryRequestInfo>
    {
        Dictionary<int, Action<MyTcpSession, string>> __handlerMap = new Dictionary<int, Action<MyTcpSession, string>>();
        CommonHandler __commonHandler = new CommonHandler();

        void RegistHandler()
        {
            // TODO : 패킷 추가되면 각 핸들러에 맞는 함수에 추가해 나갈 것
            CommonHandler();
        }

        void CommonHandler()
        {
            __handlerMap.Add((int)PacketID.DummyChatReq, __commonHandler.RequestDummyChat);
        }
    }
}
