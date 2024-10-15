using MessagePack;
using SuperSocketShared.Packet;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace SuperSocketClient.Network
{
    public abstract class SocketSession
    {
        private Socket? __socket = null;

        bool IsThreadRunning = false;
        private Thread? __packetRecvThread = null;
        private Thread? __packetSendThread = null;

        private ConcurrentQueue<byte[]> __sendPacketQueue = new ConcurrentQueue<byte[]>();

        public bool IsConnected
        {
            get { return __socket?.Connected ?? false;  }
        }

        public SocketSession()
        {
        }

        ~SocketSession()
        {
            Close();
        }

        protected bool Connect(string ip, int port)
        {
            try
            {
                __socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                __socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                if (__socket.Connected == false)
                {
                    __socket = null;
                    return false;
                }

                IsThreadRunning = true;
                __packetRecvThread = new Thread(RecvPackProcess)
                {
                    IsBackground = true
                };
                __packetRecvThread.Start();

                __packetSendThread = new Thread(SendPacketProcess)
                {
                    IsBackground = true
                };
                __packetSendThread.Start();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Tuple<int, byte[]>? Receive()
        {
            try
            {
                if (IsConnected == false)
                {
                    return null;
                }

                byte[] buffer = new byte[65536];
                var receiveSize = __socket?.Receive(buffer, 0, buffer.Length, SocketFlags.None) ?? 0;
                if (receiveSize == 0)
                {
                    return null;
                }

                return Tuple.Create(receiveSize, buffer);
            }
            catch (SocketException)
            {
            }

            return null;
        }

        private void RecvPackProcess()
        {
            while (IsThreadRunning)
            {
                if (IsConnected == false)
                {
                    Thread.Sleep(1);
                    continue;
                }

                var recvData = Receive();
                if (recvData == null)
                {
                    continue;
                }

                var receiveSize = recvData.Item1;
                if (receiveSize == 0)
                {
                    continue;
                }

                byte[] readBuffer = new byte[receiveSize];

                MemoryStream stream = new MemoryStream(recvData.Item2);
                stream.Read(readBuffer, 0, readBuffer.Length);

                int packetSize = SocketPacket.DecodeIntFromBytes(readBuffer, 0);
                if (packetSize == 0)
                {
                    continue;
                }

                byte[] packetBodyBuffer = new byte[packetSize];
                Buffer.BlockCopy(readBuffer, sizeof(int), packetBodyBuffer, 0, packetSize);
                if (packetBodyBuffer.Length > 0)
                {
                    SocketPacket socketPacket = new SocketPacket(packetBodyBuffer);
                    if (socketPacket != null)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(PacketProcess), socketPacket);
                    }
                }
            }
        }

        private void PacketProcess(object? packetObj)
        {
            if (packetObj != null)
            {
                PacketProcess((SocketPacket)packetObj);
            }
        }

        protected abstract void PacketProcess(SocketPacket packet);

        private void SendPacketProcess()
        {
            while (IsThreadRunning)
            {
                if (IsConnected)
                {
                    while (__sendPacketQueue.TryDequeue(out var sendBuffer))
                    {
                        if (sendBuffer != null)
                        {
                            Send(sendBuffer);
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }

        private void Send(byte[] sendBuffer)
        {
            try
            {
                __socket?.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
            }
        }

        protected void SendPacket<T>(PacketID packetID, T packetObj)
        {
            if (IsConnected == false || packetObj == null)
            {
                return;
            }

            SocketPacket packet = new SocketPacket((int)PacketID.DummyChatReq);
            packet.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(packetObj));

            byte[] buffer = packet.GetBytes();
            if (buffer != null)
            {
                __sendPacketQueue.Enqueue(buffer);

                //Send(buffer);
            }
        }

        protected void Close()
        {
            if (IsConnected)
            {
                __socket?.Shutdown(SocketShutdown.Both);
                __socket?.Close();
                __socket = null;

                if(IsThreadRunning)
                {
                    IsThreadRunning = false;
                    if (__packetRecvThread != null)
                    {
                        if (__packetRecvThread.IsAlive)
                            __packetRecvThread.Join();
                        __packetRecvThread = null;
                    }

                    if (__packetSendThread != null)
                    {
                        if (__packetSendThread.IsAlive)
                            __packetSendThread.Join();
                        __packetSendThread = null;

                        __sendPacketQueue.Clear();
                    }
                }            
            }
        }
    }
}
