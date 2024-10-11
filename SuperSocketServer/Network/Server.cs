using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using System;

namespace SuperSocketServer.Network
{
    // 모든 AppSession 객체를 관리, SuperSocket의 몸통
    public partial class MyServer : AppServer<MySession, MyBinaryRequestInfo>
    {
        IServerConfig __config;

        public MyServer()
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, MyBinaryRequestInfo>())
        {
            NewSessionConnected += new SessionHandler<MySession>(OnConnected);
            SessionClosed += new SessionHandler<MySession, CloseReason>(OnClosed);
            NewRequestReceived += new RequestHandler<MySession, MyBinaryRequestInfo>(RequestReceived);
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

        static void OnConnected(MySession session)
        {
            Console.WriteLine($"세션 {session.SessionID} 접속.");
        }

        static void OnClosed(MySession session, CloseReason reason)
        {
            Console.WriteLine($"세션 {session.SessionID} 접속 해제 : {reason}");
        }

        void RequestReceived(MySession session, MyBinaryRequestInfo requestInfo)
        {
            if (__handlerMap.TryGetValue(requestInfo.Head, out var Value))
            {
                Value(session, requestInfo);
            }
            else
            {
                Console.WriteLine($"세션 {session.SessionID} 받은 데이터 크기 : {requestInfo.Body.Length}");
            }
        }
    }
}
