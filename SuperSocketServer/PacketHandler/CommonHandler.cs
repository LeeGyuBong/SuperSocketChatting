using MessagePack;
using Newtonsoft.Json;
using SuperSocketServer.Network.TCP;
using SuperSocketShared.Packet;
using System;
using System.Net.Sockets;
using System.Text;

namespace SuperSocketServer.PacketHandler
{
    public class CommonHandler
    {
        public void RequestDummyChat(MyTcpSession session, string packetData)
        {
            PKSendChatMessage packet = MessagePackSerializer.Deserialize<PKSendChatMessage>(Convert.FromBase64String(packetData));
            if(packet != null)
            {
                Console.WriteLine($"[{packet.Sender}] Message : {packet.Message}");

                PKSendChatMessage pKSendChatMessage = new PKSendChatMessage();
                pKSendChatMessage.Sender = packet.Sender;
                pKSendChatMessage.Message = packet.Message;

                session.SendPacket(PacketID.DummyChatReq, pKSendChatMessage);
            }
        }
    }
}
