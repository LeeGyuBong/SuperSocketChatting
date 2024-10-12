using MessagePack;
using SuperSocketClient.Network;
using SuperSocketShared.Packet;

namespace SuperSocketClient.Object
{
    public class Player
    {
        private readonly TcpConnection __tcpConnection = new TcpConnection();

        public string Name { get; init; }

        public Player(string name) 
        {   
            Name = name;
        }

        ~Player()
        {
            if (__tcpConnection.IsConnected())
            {
                __tcpConnection.Close();
            }
        }

        public bool Login(string ip, int port)
        {
            bool result = false;
            if (__tcpConnection.IsConnected() == false)
            {
                result = __tcpConnection.Connect(ip, port);
            }

            return result;
        }

        public void Logout()
        {
            if(__tcpConnection.IsConnected())
            {
                __tcpConnection.Close();
            }
        }

        private void SendPacket<T>(PacketID packetID, T packetObj)
        {
            if(__tcpConnection.IsConnected() && packetObj != null)
            {
                SocketPacket packet = new SocketPacket((int)packetID);
                packet.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(packetObj));

                byte[] buffer = packet.GetBytes();
                if (buffer != null)
                {
                    __tcpConnection.Send(buffer);
                }
            }
        }

        public void SendChat(string message)
        {
            if(__tcpConnection.IsConnected() == false)
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
