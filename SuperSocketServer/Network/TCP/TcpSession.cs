﻿using MessagePack;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketServer.Network.TCP
{
    public class MyTcpSession : AppSession<MyTcpSession, BinaryRequestInfo>
    {
        // 서버에 연결되는 Socket 로직 클래스
        // 해당 클래스로 클라이언트와의 연결, 해제, 데이터 입출력을 한다

        public bool IsConnected
        {
            get
            {
                return Connected;
            }
        }

        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }

        public void SendPacket<T>(PacketID packetID, T packetObj)
        {
            if (IsConnected && packetObj != null)
            {
                SocketPacket packet = new SocketPacket((int)packetID);

                try
                {
                    packet.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(packetObj));

                    byte[] buffer = packet.GetBytes();
                    if (buffer != null)
                    {
                        TrySend(new ArraySegment<byte>(buffer));
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
