using MessagePack;
using SuperSocketServer.Network.TCP;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketServer.PacketHandler
{
    public class CommonHandler
    {
        public void RequestDummyChat(MyTcpSession session, string packetData)
        {
            PKSendChatMessage packet = MessagePackSerializer.Deserialize<PKSendChatMessage>(Convert.FromBase64String(packetData));
            if (packet != null)
            {
                Console.WriteLine($"[{packet.Sender}] Message : {packet.Message}");

                PKSendChatMessage pKSendChatMessage = new PKSendChatMessage();
                pKSendChatMessage.Sender = packet.Sender;
                pKSendChatMessage.Message = packet.Message;

                session.SendPacket(PacketID.DummyChatReq, pKSendChatMessage);

                // TODO : MyTcpServer를 가져와서 브로드캐스팅으로 변경
            }
        }
    }
}
