using MessagePack;
using SuperSocketClient.Network;
using SuperSocketClient.Scene;
using SuperSocketShared.Packet;

namespace SuperSocketClient.Object
{
    public partial class Client
#if LOCAL_SOCKET
        : TcpConnection
#else
        : ClientSession
#endif
    {
        public bool IsInit { get; private set; } = false;
        public string Name { get; private set; }

        public void Init(string name)
        {
            Name = name;

            IsInit = true;
        }

        public void Reset()
        {
            Name = "";

            IsInit = false;
        }

        public bool Login()
        {
#if LOCAL_SOCKET
            return Connect("127.0.0.1", 11021);
#else
            return Connected();
#endif
        }

        public void Logout()
        {
            Reset();

#if LOCAL_SOCKET
            Close();
#else
            Disconnected();
#endif
        }

        public void SendChat(string message)
        {

            if (IsConnected == false)
            {
                return;
            }

            var packetBody = new PKSendChatMessage()
            {
                Sender = Name,
                Message = message
            };

            SendPacket(PacketID.DummyChatReq, packetBody);
        }

        protected override void PacketProcess(SocketPacket packet)
        {
            if (packet == null)
                return;

            switch ((PacketID)packet.Type)
            {
                case PacketID.DummyChatReq:
                    {
                        PKSendChatMessage pkSendChatMessage = MessagePackSerializer.Deserialize<PKSendChatMessage>(Convert.FromBase64String(packet.Data));
                        if (pkSendChatMessage != null)
                        {
                            ChatForm chatForm = FormManager.Instance().GetForm(FormType.Chat) as ChatForm;
                            if (chatForm != null)
                            {
                                chatForm.BoradCastChatBox(pkSendChatMessage.Sender, pkSendChatMessage.Message);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
