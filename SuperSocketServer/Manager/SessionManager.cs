using MessagePack;
using SuperSocketServer.Network.TCP;
using SuperSocketServer.Utility;
using SuperSocketShared.Packet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSocketServer.Manager
{
    public class SessionManager : Singleton<SessionManager>
    {
        private int __sessionId = 0;

        private ConcurrentDictionary<Int64/*UID*/, SocketSession> __sessionDic = new ConcurrentDictionary<Int64, SocketSession>();

        public SocketSession GetSession(Int64 sessionId)
        {
            if (__sessionDic.TryGetValue(sessionId, out var session))
            {
                return session;
            }

            return null;
        }

        public SocketSession GetSession(string sessionName)
        {
            foreach (SocketSession socketSession in __sessionDic.Values)
            {
                if (socketSession.UserName == sessionName)
                {
                    return socketSession;
                }
            }

            return null;
        }

        public void InsertSession(SocketSession session)
        {
            if (__sessionDic.ContainsKey(session.UID) == false)
            {
                var uid = Interlocked.Increment(ref __sessionId);

                session.SetUID(uid);
                __sessionDic.TryAdd(uid, session);
            }
        }

        public void RemoveSession(Int64 sessionId)
        {
            if(__sessionDic.TryRemove(sessionId, out var session))
            {
                if (string.IsNullOrEmpty(session.UserName) == false)
                {
                    PKBroadcastLogoutAck ack = new PKBroadcastLogoutAck
                    {
                        UserName = session.UserName
                    };

                    SocketPacket ackPacket = new SocketPacket((int)PacketID.BroadcastLogoutAck, Convert.ToBase64String(MessagePackSerializer.Serialize(ack)));
                    SendAll(ackPacket.GetBytes());
                }
            }
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

        public void GetSessionNameList(out List<string> list)
        {
            list = new List<string>();

            foreach (var session in __sessionDic.Values)
            {
                list.Add(session.UserName);
            }
        }
    }
}
