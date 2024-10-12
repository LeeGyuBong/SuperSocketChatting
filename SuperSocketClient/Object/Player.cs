using SuperSocketClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketClient.Object
{
    public class Player
    {
        private readonly TcpConnection __tcpConnection = new TcpConnection();

        public Player() 
        {
        }

        ~Player()
        {
            Logout();
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
    }
}
