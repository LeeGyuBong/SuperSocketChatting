using MessagePack;
using SuperSocketShared.Packet;
using System.Net;
using System.Net.Sockets;

namespace SuperSocketClient.Network
{
    public abstract class TcpConnection 
    {
        public Socket? socket = null;
        public string LatestErrorMsg = "";

        bool IsThreadRunning = false;
        private Thread? __packetRecvThread = null;

        public bool IsConnected
        {
            get { return socket != null && socket.Connected; }
        }

        public TcpConnection()
        {
        }

        ~TcpConnection()
        {
            Close();
        }

        protected bool Connect(string ip, int port)
        {
            try
            {
                IPAddress serverIP = IPAddress.Parse(ip);
                int serverPort = port;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (socket == null)
                {
                    return false;
                }

                socket.Connect(new IPEndPoint(serverIP, serverPort));
                if (socket.Connected == false)
                {
                    return false;
                }

                socket.LingerState = new LingerOption(true, 0);

                IsThreadRunning = true;
                __packetRecvThread = new Thread(RecvPackProcess);
                __packetRecvThread.IsBackground = true;
                __packetRecvThread.Start();

                return true;
            }
            catch(Exception ex)
            {
                LatestErrorMsg = ex.Message;
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
                var receiveSize = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                if(receiveSize == 0)
                {
                    return null;
                }

                return Tuple.Create(receiveSize, buffer);
            }
            catch(SocketException se)
            {
                LatestErrorMsg = se.Message;
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

        private void Send(byte[] sendBuffer) 
        {
            try
            {
                socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.Message;
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
                Send(buffer);
            }
        }

        protected void Close()
        {
            if (IsConnected) 
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;

                if (__packetRecvThread != null && __packetRecvThread.IsAlive)
                {
                    IsThreadRunning = false;
                    __packetRecvThread.Join();
                    __packetRecvThread = null;
                }
            }
        }
    }
}
