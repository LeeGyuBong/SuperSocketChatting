using MessagePack;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketServer.Network.TCP
{
    // SuperSocket을 사용한 TCP 서버
    // 모든 AppSession 객체를 관리, SuperSocket의 몸통
    public partial class MyTcpServer : AppServer<MyTcpSession, BinaryRequestInfo>
    {
        IServerConfig __config;

        public int MaxPacketSize = 65536;

        public MyTcpServer()
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, BinaryRequestInfo>())
        {
            //NewSessionConnected += new SessionHandler<MyTcpSession>(OnConnected);
            //SessionClosed += new SessionHandler<MyTcpSession, CloseReason>(OnClosed);
            NewRequestReceived += new RequestHandler<MyTcpSession, BinaryRequestInfo>(OnRequestReceived);
        }

        public bool InitConfig()
        {
            // TODO : Json 형태로 변경
            __config = new ServerConfig
            {
                Port = 11021,
                Ip = "Any",
                MaxConnectionNumber = 100,
                Mode = SocketMode.Tcp,
                Name = "SuperSocketServer"
            };

            return __config != null ? true : false;
        }

        public bool SetupServer()
        {
            try
            {
                if (false == Setup(new RootConfig(), __config, logFactory: new ConsoleLogFactory()))
                {
                    return false;
                }

                RegistHandler();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception!! Message : {ex.Message}, StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        //static void OnConnected(MyTcpSession session)
        //{
        //    Console.WriteLine($"세션 {session.SessionID} 접속.");
        //}

        //static void OnClosed(MyTcpSession session, CloseReason reason)
        //{
        //    Console.WriteLine($"세션 {session.SessionID} 접속 해제 : {reason}");
        //}

        void OnRequestReceived(MyTcpSession session, BinaryRequestInfo requestInfo)
        {
            if (requestInfo.Body.Length <= MaxPacketSize)
            {
                SocketPacket packet = new SocketPacket(requestInfo.Body);
                if (packet != null)
                {
                    if (__handlerMap.TryGetValue(packet.Type, out var Value))
                    {
                        Value(session, packet.Data);
                    }
                    else
                    {
                        Console.WriteLine($"Handler Not Exist! 세션 {session.SessionID} 받은 데이터 크기 : {requestInfo.Body.Length}");
                    }
                }
            }
        }
    }
}
