using MessagePack;
using SuperSocketClient.Manager;
using SuperSocketClient.Network;
using SuperSocketClient.Scene;
using SuperSocketShared.Packet;

namespace SuperSocketClient.Object
{
    public class Client
    {
        public SocketSessionType SocketSessionType { get; set; } = SocketSessionType.SuperSocket;
        ISession? __session = null;

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
                if (SocketSessionType  == SocketSessionType.SuperSocket)
                    __session = new SuperSocketSession();
                else if (SocketSessionType == SocketSessionType.NetSocket)
                    __session = new SocketSession();

                __session?.AddPacketHandler(PacketID.LoginAck, ProcessLoginAck);
                __session?.AddPacketHandler(PacketID.BroadcastLoginAck, ProcessBroadcastLoginAck);
                __session?.AddPacketHandler(PacketID.BroadcastLogoutAck, ProcessBroadcastLogoutAck);
                __session?.AddPacketHandler(PacketID.BroadcastChatAck, ProcessBroadcastChatAck);
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

        public void Logout()
        {
            Reset();

            __session?.CloseSession();
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
                return;

            if (string.IsNullOrEmpty(message) ||
                string.IsNullOrWhiteSpace(message))
                return;

            PKChatReq chatReqPacket = new PKChatReq()
            {
                UserName = Name,
                Message = message
            };

            __session?.SendPacket(PacketID.ChatReq, chatReqPacket);
        }

        public void ProcessLoginAck(SocketPacket recvPacket)
        {
            if (recvPacket == null)
                return;

            PKLoginAck packet = MessagePackSerializer.Deserialize<PKLoginAck>(Convert.FromBase64String(recvPacket.Data));
            if (packet == null)
                return;

            var form = FormManager.Instance.GetForm(FormType.Login) as LoginForm;

            if (packet.ErrorEvent != ErrorEvent.None)
            {
                switch(packet.ErrorEvent)
                {
                    case ErrorEvent.DuplicateLogin:
                        form?.ErrorLabelTextUpdateEventHandler.Invoke(this, "중복 아이디 접속 시도");
                        break;
                }
               
                Logout();
                return;
            }

            UID = packet.UID;

            // 채팅 씬으로 전환
            form?.ChangeFormEventHandler.Invoke(this, EventArgs.Empty);
        }

        public void ProcessBroadcastLoginAck(SocketPacket recvPacket)
        {
            if (recvPacket == null)
                return;

            PKBroadcastLoginAck packet = MessagePackSerializer.Deserialize<PKBroadcastLoginAck>(Convert.FromBase64String(recvPacket.Data));
            if (packet == null)
                return;

            ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
            chatForm?.ChatBoxWriteEventHandler.Invoke(this, new BroadcastChatBoxData()
            {
                Message = $"{packet.UserName}님이 로그인했습니다."
            });
        }

        public void ProcessBroadcastLogoutAck(SocketPacket recvPacket)
        {
            if (recvPacket == null)
                return;

            PKBroadcastLogoutAck packet = MessagePackSerializer.Deserialize<PKBroadcastLogoutAck>(Convert.FromBase64String(recvPacket.Data));
            if (packet == null)
                return;

            ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
            chatForm?.ChatBoxWriteEventHandler.Invoke(this, new BroadcastChatBoxData()
            {
                Message = $"{packet.UserName}님이 로그아웃했습니다."
            });
        }

        public void ProcessBroadcastChatAck(SocketPacket recvPacket)
        {
            if (recvPacket == null)
                return;

            PKBroadcastChatAck packet = MessagePackSerializer.Deserialize<PKBroadcastChatAck>(Convert.FromBase64String(recvPacket.Data));
            if (packet == null)
                return;

            ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
            chatForm?.ChatBoxWriteEventHandler.Invoke(this, new BroadcastChatBoxData()
            {
                Sender = packet.UserName,
                Message = packet.Message
            });
        }
    }
}
