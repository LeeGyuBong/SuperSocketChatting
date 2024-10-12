using System.Net;
using System.Net.Sockets;

namespace SuperSocketClient.Network
{
    internal class TcpConnection
    {
        public Socket? socket = null;
        public string LatestErrorMsg = "";

        public bool IsConnected() { return socket != null && socket.Connected; }

        public bool Connect(string ip, int port)
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

                return true;
            }
            catch(Exception ex)
            {
                LatestErrorMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>int : Size, byte[] : Value</returns>
        public Tuple<int, byte[]>? Receive()
        {
            try
            {
                if(socket == null)
                {
                    return null;
                }

                byte[] buffer = new byte[2048];

                var receiveData = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                if(receiveData == 0)
                {
                    return null;
                }

                return Tuple.Create(receiveData, buffer);
            }
            catch(SocketException se)
            {
                LatestErrorMsg = se.Message;
            }

            return null;
        }

        public void Send(byte[] sendBuffer) 
        {
            try
            {
                if(socket == null || socket.Connected == false)
                {
                    LatestErrorMsg = "서버에 연결이 되어있지 않음!";
                    return;
                }

                socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.Message;
            }
        }

        public void Close()
        {
            if(socket != null && socket.Connected) 
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
