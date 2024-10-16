using SuperSocketServer.Network.TCP;
using SuperSocketServer.Utility;
using System;
using System.Collections.Concurrent;

namespace SuperSocketServer.Manager
{
    public class SessionManager : Singleton<SessionManager>
    {
        private Int64 __sessionId = 0;

        private ConcurrentDictionary<Int64/*UID*/, SocketSession> __sessionDic = new ConcurrentDictionary<Int64, SocketSession>();

        public SocketSession GetSession(Int64 sessionId)
        {
            if (__sessionDic.TryGetValue(sessionId, out var session))
            {
                return session;
            }

            return null;
        }

        public void InsertSession(SocketSession session)
        {
            if (__sessionDic.ContainsKey(session.UID) == false)
            {
                session.SetUID(++__sessionId);

                __sessionDic.TryAdd(__sessionId, session);
            }
        }

        public bool RemoveSession(Int64 sessionId)
        {
            return __sessionDic.TryRemove(__sessionId, out var session);
        }

        public void SendAll(byte[] sendBuffer)
        {
            if (sendBuffer == null || sendBuffer.Length <= 0)
                return;

            foreach (var session in __sessionDic.Values)
            {
                session.SendPacket(sendBuffer);
            }
        }
    }
}
