using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;

namespace SuperSocketStudy.SuperSocket
{
    public class TelnetServer : AppServer<TelnetSession>
    {
        // 모든 AppSession 객체를 관리, SuperSocket의 몸통

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStartup()
        {
            base.OnStartup();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }

    public class TelnetSession : AppSession<TelnetSession>
    {
        // 서버에 연결되는 Socket 로직 클래스
        // 해당 클래스로 클라이언트와의 연결, 해제, 데이터 입출력을 한다

        protected override void OnSessionStarted()
        {
            Send("Welcome to SuperSocket Telnet Server!");
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Send("Unknow request");
        }

        protected override void HandleException(Exception e)
        {
            Send("Application error: {0}", e.Message);
        }
    }
}
