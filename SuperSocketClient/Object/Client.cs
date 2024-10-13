using SuperSocketShared.Packet;

namespace SuperSocketClient.Object
{
    public partial class Client
    {
        public string Name { get; init; }

        public Client(string name) 
        {   
            Name = name;
        }

        ~Client()
        {
            Logout();
        }

        public bool Login()
        {
            return Connected();
        }

        public void Logout()
        {
            Disconnected();
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
    }
}
