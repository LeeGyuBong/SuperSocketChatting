using SuperSocket.SocketBase;
using SuperSocketServer.Packet;
using SuperSocketServer.PacketHandler;
using System;
using System.Collections.Generic;

namespace SuperSocketServer.Network.TCP
{
    // 패킷이 많아지면 Server.cs의 가독성이 떨어질 수 있음
    // 그래서 패킷 등록 부분만 따로 분리함
    public partial class MyServer : AppServer<MySession, MyBinaryRequestInfo>
    {
        Dictionary<int, Action<MySession, MyBinaryRequestInfo>> __handlerMap = new Dictionary<int, Action<MySession, MyBinaryRequestInfo>>();
        CommonHandler __commonHanlder = new CommonHandler();

        void RegistHandler()
        {
            // TODO : 패킷 추가되면 아래에 추가해 나갈 것

            __handlerMap.Add((int)PacketID.PacketID_DummyChatReq, __commonHanlder.RequestDummyChat);
        }
    }
}
