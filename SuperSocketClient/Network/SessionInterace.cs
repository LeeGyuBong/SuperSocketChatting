using SuperSocketShared.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
