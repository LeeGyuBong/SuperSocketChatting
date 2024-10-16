using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketServer.PacketHandler;
using SuperSocketShared.Packet;
using System;
using System.Collections.Generic;

namespace SuperSocketServer.Network.TCP
{
    // 패킷이 많아지면 Server.cs의 가독성이 떨어질 수 있기에 패킷 등록 부분만 따로 분리
    public partial class SocketServer : AppServer<SocketSession, BinaryRequestInfo>
    {
        Dictionary<int, Action<SocketSession, string>> __packetHandlerDic = new Dictionary<int, Action<SocketSession, string>>();
        CommonHandler __commonHandler = new CommonHandler();

        void OnNewRequestReceived(SocketSession session, BinaryRequestInfo requestInfo)
        {
            if (requestInfo.Body.Length <= MaxPacketSize)
            {
                SocketPacket packet = new SocketPacket(requestInfo.Body);
                if (packet != null)
                {
                    if (__packetHandlerDic.TryGetValue(packet.Type, out var action))
                    {
                        action?.Invoke(session, packet.Data);
                    }
                    else
                    {
                        Console.WriteLine($"Handler Not Exist! 세션 {session.SessionID} 받은 데이터 크기 : {requestInfo.Body.Length}");
                    }
                }
            }
        }

        void RegistHandler()
        {
            // TODO : 패킷 추가되면 각 핸들러에 맞는 함수에 추가해 나갈 것
            CommonHandler();
        }

        void CommonHandler()
        {
            __packetHandlerDic.Add((int)PacketID.LoginReq, __commonHandler.ProcessLoginReq);
            __packetHandlerDic.Add((int)PacketID.LoadCompletedReq, __commonHandler.ProcessLoadCompletedReq);
            __packetHandlerDic.Add((int)PacketID.ChatReq, __commonHandler.ProcessChatReq);
        }
    }
}
