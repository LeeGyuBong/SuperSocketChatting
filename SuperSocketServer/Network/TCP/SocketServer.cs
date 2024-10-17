using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using SuperSocketServer.Manager;
using System;

namespace SuperSocketServer.Network.TCP
{
    // SuperSocket을 사용한 TCP 서버
    // 모든 AppSession 객체를 관리, SuperSocket의 몸통
    public partial class SocketServer : AppServer<SocketSession, BinaryRequestInfo>
    {
        IServerConfig __config;

        public int MaxPacketSize = 65536;

        public SocketServer()
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, BinaryRequestInfo>())
        {
            NewSessionConnected += new SessionHandler<SocketSession>(OnConnected);
            SessionClosed += new SessionHandler<SocketSession, CloseReason>(OnClosed);
            NewRequestReceived += new RequestHandler<SocketSession, BinaryRequestInfo>(OnNewRequestReceived);
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
                Console.WriteLine($"[SocketServer] SetupServer - Exception.(Message:{ex.Message},Trace:{ex.StackTrace})");
                return false;
            }
        }

        public void OnConnected(SocketSession session)
        {
            SessionManager.Instance.InsertSession(session);
        }

        static void OnClosed(SocketSession session, CloseReason reason)
        {
            SessionManager.Instance.RemoveSession(session.UID);
        }
    }
}
