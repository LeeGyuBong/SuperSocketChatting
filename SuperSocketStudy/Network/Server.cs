using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using SuperSocketStudy.PacketHandler;
using System;
using System.Collections.Generic;

namespace SuperSocketStudy.Network
{
    public class MyServer : AppServer<MySession, MyBinaryRequestInfo>
    {
        Dictionary<int, Action<MySession, MyBinaryRequestInfo>> __handlerMap = new Dictionary<int, Action<MySession, MyBinaryRequestInfo>>();
        CommonHandler __commonHanlder = new CommonHandler();

        IServerConfig __config;

        // 모든 AppSession 객체를 관리, SuperSocket의 몸통

        public MyServer()
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, MyBinaryRequestInfo>())
        {
            NewSessionConnected += new SessionHandler<MySession>(OnConnected);
            SessionClosed += new SessionHandler<MySession, CloseReason>(OnClosed);
            NewRequestReceived += new RequestHandler<MySession, MyBinaryRequestInfo>(RequestReceived);
        }

        void RegistHandler()
        {
            // TODO : 패킷 추가되면 아래에 추가해 나갈 것

            __handlerMap.Add((int)PacketID.PacketID_DummyChatReq, __commonHanlder.RequestDummyChat);
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
                Name = "SuperSocketStudy"
            };

            return __config != null ? true : false;
        }

        public bool SetupServer()
        {
            try
            {
                if (false == Setup(new RootConfig(), __config, logFactory: new ConsoleLogFactory()))
                {
                    throw new Exception();
                }

                RegistHandler();

                return true;
            }
            catch (Exception)
            {
                // TODO : Log
                return false;
            }
        }

        static void OnConnected(MySession session)
        {

        }

        static void OnClosed(MySession session, CloseReason reason)
        {
        }

        void RequestReceived(MySession session, MyBinaryRequestInfo requestInfo)
        {
            if (__handlerMap.TryGetValue(requestInfo.Head, out var Value))
            {
                Value(session, requestInfo);
            }
            else
            {
                // TODO : Log
            }
        }
    }
}
