using MessagePack;
using SuperSocketClient.Manager;
using SuperSocketClient.Network;
using SuperSocketClient.Scene;
using SuperSocketShared.Packet;

namespace SuperSocketClient.Object
{
    public class Client
    {
        ISession? __session;

        public bool IsInit { get; private set; } = false;
        public string Name { get; private set; } = string.Empty;
        public Int64 UID { get; private set; } = 0;

        ~Client()
        {
            Logout();
        }

        public void Init(string name)
        {
            Name = name;

            if (__session == null)
            {
                __session = new SocketSession();
                //__session = new SuperSocketSession();
                __session.AddPacketHandler(PacketID.LoginAck, ProcessLoginAck);
                __session.AddPacketHandler(PacketID.BroadcastChatAck, ProcessBroadcastChatAck);
            }

            IsInit = true;
        }

        public void Reset()
        {
            Name = "";
            UID = 0;

            IsInit = false;
        }

        public bool SessionConnect()
        {
            return __session?.ConnectSession("127.0.0.1", 11021) ?? false;
        }

        public void SendLoginReq()
        {
            if (__session?.IsConnected == false)
            {
                return;
            }

            PKLoginReq loginReqPacket = new PKLoginReq()
            {
                UserName = Name
            };

            __session?.SendPacket(PacketID.LoginReq, loginReqPacket);
        }

        public void Logout()
        {
            Reset();

            __session?.CloseSession();
        }

        public void SendLoadCompleted()
        {
            if (__session?.IsConnected == false)
            {
                return;
            }

            __session?.SendPacket(PacketID.LoadCompletedReq, new PKLoadCompletedReq());
        }

        public void SendChatReq(string message)
        {
            if (__session?.IsConnected == false)
            {
                return;
            }

            PKChatReq chatReqPacket = new PKChatReq()
            {
                Sender = Name,
                Message = message
            };

            __session?.SendPacket(PacketID.ChatReq, chatReqPacket);
        }

        public void ProcessLoginAck(SocketPacket recvPacket)
        {
            if (recvPacket == null)
                return;

            PKLoginAck packet = MessagePackSerializer.Deserialize<PKLoginAck>(Convert.FromBase64String(recvPacket.Data));
            if (packet != null)
            {
                if (packet.ErrorEvent != ErrorEvent.None)
                {
                    Logout();
                    return;
                }

                UID = packet.UID;

                // 채팅 씬으로 전환
                var form = FormManager.Instance.GetForm(FormType.Login) as LoginForm;
                form?.ChangeFormEventHandler.Invoke(this, EventArgs.Empty);
            }
        }

        public void ProcessBroadcastChatAck(SocketPacket recvPacket)
        {
            if (recvPacket == null)
                return;

            PKBroadcastChatAck packet = MessagePackSerializer.Deserialize<PKBroadcastChatAck>(Convert.FromBase64String(recvPacket.Data));
            if (packet != null)
            {
                ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
                chatForm?.ChatBoxWriteEventHandler.Invoke(this, new BroadcastChatBoxData()
                {
                    Sender = packet.Sender,
                    Message = packet.Message
                });
            }
        }
    }
}
