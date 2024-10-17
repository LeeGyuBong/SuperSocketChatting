using SuperSocketShared.Packet;

namespace SuperSocketClient.Network
{
    public enum SocketSessionType
    {
        NetSocket,
        SuperSocket,
    }

    interface ISession
    {
        bool IsConnected { get; }

        bool ConnectSession(string ip, int port);

        void CloseSession();

        void SendPacket<T>(PacketID packetID, T packetObj);

        void PacketProcess(object? packetObj);

        void AddPacketHandler(PacketID packetID, Action<SocketPacket> eventHandler);
    }
}
