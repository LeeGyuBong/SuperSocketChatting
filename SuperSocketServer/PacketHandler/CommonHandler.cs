using MessagePack;
using SuperSocketServer.Manager;
using SuperSocketServer.Network.TCP;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketServer.PacketHandler
{
    public class CommonHandler
    {
        public void ProcessLoginReq(SocketSession session, string packetData)
        {
            PKLoginReq packet = MessagePackSerializer.Deserialize<PKLoginReq>(Convert.FromBase64String(packetData));
            if (packet == null)
                return;

            PKLoginAck ack = new PKLoginAck();

            if (SessionManager.Instance.GetSession(packet.UserName) != null)
            {
                ack.ErrorEvent = ErrorEvent.DuplicateLogin;
            }
            else
            {
                ack.UID = session.UID;

                session.SetUserName(packet.UserName);

                Console.WriteLine($"[CommonHandler] ProcessLoginReq - {packet.UserName}님이 로그인했습니다.");
            }
            
            session.SendPacket(PacketID.LoginAck, ack);
        }

        public void ProcessLoadCompletedReq(SocketSession session, string packetData)
        {
            PKLoadCompletedReq packet = MessagePackSerializer.Deserialize<PKLoadCompletedReq>(Convert.FromBase64String(packetData));
            if (packet == null)
                return;

            PKBroadcastLoginAck ack = new PKBroadcastLoginAck
            {
                UserName = session.UserName
            };

            SocketPacket ackPacket = new SocketPacket((int)PacketID.BroadcastLoginAck, Convert.ToBase64String(MessagePackSerializer.Serialize(ack)));
            SessionManager.Instance.SendAll(ackPacket.GetBytes());
        }

        public void ProcessChatReq(SocketSession session, string packetData)
        {
            PKChatReq packet = MessagePackSerializer.Deserialize<PKChatReq>(Convert.FromBase64String(packetData));
            if (packet == null)
                return;

            PKBroadcastChatAck ack = new PKBroadcastChatAck();
            ack.UserName = packet.UserName;
            ack.Message = packet.Message;

            SocketPacket ackPacket = new SocketPacket((int)PacketID.BroadcastChatAck);
            ackPacket.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(ack));

            SessionManager.Instance.SendAll(ackPacket.GetBytes());
        }
    }
}
