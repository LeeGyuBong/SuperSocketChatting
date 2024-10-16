using MessagePack;
using SuperSocketClient.Manager;
using SuperSocketClient.Network;
using SuperSocketClient.Scene;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketClient.Object
{
    public class Client
    {
        ISession? __session;

        public bool IsInit { get; private set; } = false;
        public string Name { get; private set; } = string.Empty;

        public void Init(string name)
        {
            Name = name;

            if(__session == null)
            {
                //__session = new SocketSession();
                __session = new SuperSocketSession();
                __session.AddPacketProcessEvent(PacketID.DummyChatReq, new EventHandler<SocketPacket>(PKSendChatMessageProcess));
            }

            IsInit = true;
        }

        public void Reset()
        {
            Name = "";

            IsInit = false;
        }

        public bool Login()
        {
            return __session?.ConnectSession("127.0.0.1", 11021) ?? false;
        }

        public void Logout()
        {
            Reset();

            __session?.CloseSession();
        }

        public void SendChat(string message)
        {
            if (__session?.IsConnected == false)
            {
                return;
            }

            var packetBody = new PKSendChatMessage()
            {
                Sender = Name,
                Message = message
            };

            __session?.SendPacket(PacketID.DummyChatReq, packetBody);
        }

        public void PKSendChatMessageProcess(object? sender, SocketPacket packet)
        {
            if (packet == null)
                return;

            PKSendChatMessage pkSendChatMessage = MessagePackSerializer.Deserialize<PKSendChatMessage>(Convert.FromBase64String(packet.Data));
            if (pkSendChatMessage != null)
            {
                ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
                chatForm?.BoradCastChatBox(pkSendChatMessage.Sender, pkSendChatMessage.Message);
            }
        }
    }
}
