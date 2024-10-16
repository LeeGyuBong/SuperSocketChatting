using SuperSocketShared.Packet;

namespace SuperSocketClient.Network
{
    interface ISession
    {
        bool IsConnected { get; }

        bool ConnectSession(string ip, int port);

        void CloseSession();

        void SendPacket<T>(PacketID packetID, T packetObj);

        void PacketProcess(object? packetObj);

        void AddPacketProcessEvent(PacketID packetID, Action<SocketPacket> eventHandler);
    }
}
