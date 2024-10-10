using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketStudy.Server
{
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
