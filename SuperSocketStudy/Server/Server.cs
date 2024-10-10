using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using SuperSocketStudy.Server;
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

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }
}
