using MessagePack;
using SuperSocket.ClientEngine;
using SuperSocketShared.Packet;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace SuperSocketClient.Network
{
    public class SuperSocketSession : ISession
    {
        private TcpClientSession? __tcpSession = null;

        private int __receivedSize = 0;
        private int __packetSize = 0;
        private byte[] __receiveBuffer = null;
        private byte[] __header = null;

        private ConcurrentDictionary<int, EventHandler<SocketPacket>> __packetProcess = new ConcurrentDictionary<int, EventHandler<SocketPacket>>();

        ~SuperSocketSession()
        {
            CloseSession();
        }

        // ---------------------------------------------------------------------------
        public bool IsConnected
        {
            get { return __tcpSession?.IsConnected ?? false; }
        }

        public bool ConnectSession(string ip, int port)
        {
            try
            {
                if (__tcpSession == null)
                {
                    __tcpSession = new AsyncTcpSession
                    {
                        ReceiveBufferSize = 65536,
                        NoDelay = true
                    };

                    __tcpSession.Connected += OnConnected;
                    __tcpSession.DataReceived += OnDataReceive;
                    __tcpSession.Closed += OnClosed;
                    __tcpSession.Error += OnError;
                }

                if (__tcpSession.IsConnected == false)
                {
                    __tcpSession.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client] Connect - Exception.(Message:{ex.Message},Trace:{ex.StackTrace})");
                return false;
            }
        }

        public void CloseSession()
        {
            if (IsConnected)
            {
                __tcpSession?.Close();
            }
        }

        public void SendPacket<T>(PacketID packetID, T packetObj)
        {
            if (IsConnected == false || packetObj == null)
            {
                return;
            }

            SocketPacket packet = new SocketPacket((int)packetID);
            packet.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(packetObj));

            byte[] buffer = packet.GetBytes();
            if (buffer != null)
            {
                __tcpSession?.Send(new ArraySegment<byte>(buffer));
            }
        }

        public void PacketProcess(object? packetObj)
        {
            if (packetObj != null)
            {
                SocketPacket packet = (SocketPacket)packetObj;
                if (__packetProcess?.TryGetValue(packet.Type, out var eventHandler) ?? false)
                {
                    eventHandler?.Invoke(this, packet);
                }
            }
        }

        public void AddPacketProcessEvent(PacketID packetID, EventHandler<SocketPacket> eventHandler)
        {
            __packetProcess?.TryAdd((int)packetID, eventHandler);
        }
        // ---------------------------------------------------------------------------

        private void OnConnected(object? sender, EventArgs e)
        {

        }

        private void OnDataReceive(object? sender, DataEventArgs e)
        {
            if (e.Length <= 0)
                return;

            try
            {
                MemoryStream stream = new MemoryStream(e.Data);
                byte[] buffer = new byte[e.Length];
                stream.Read(buffer, e.Offset, e.Length);

                if (__packetSize == 0)
                {
                    ReceiveHeader(buffer, 0, e.Length);
                }
                else
                {
                    ReceiveBody(buffer, 0, e.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client] OnDataReceive - Exception.(Message:{ex.Message},Trace:{ex.StackTrace})");
            }
        }

        private void ReceiveHeader(byte[] readBuffer, int offset, int length)
        {
            int rest = __receivedSize + length - SocketPacket.PACKET_LENGTH_SIZE;
            if (rest >= 0)
            {
                if (__header == null)
                {
                    // 1회 통신으로 데이터를 다 받은 경우
                    __packetSize = SocketPacket.DecodeIntFromBytes(readBuffer, offset);
                    __receivedSize += SocketPacket.PACKET_LENGTH_SIZE;
                }
                else
                {
                    int recvLength = SocketPacket.PACKET_LENGTH_SIZE - __receivedSize;
                    Buffer.BlockCopy(readBuffer, offset, __header, __receivedSize, recvLength);
                    __packetSize = SocketPacket.DecodeIntFromBytes(__header, 0);
                    __receivedSize += recvLength;
                    __header = null;
                }

                __receiveBuffer = new byte[__packetSize];
                __receivedSize = 0;

                if (rest > 0)
                {
                    ReceiveBody(readBuffer, offset + length - rest, rest);
                }
            }
            else
            {
                // 계속 데이터 받아야함
                if (__receivedSize == 0)
                {
                    __header = new byte[SocketPacket.PACKET_LENGTH_SIZE];
                }

                Buffer.BlockCopy(readBuffer, 0, __header, __receivedSize, length);
                __receivedSize += length;
            }
        }

        private void ReceiveBody(byte[] readBuffer, int offset, int length)
        {
            int rest = __receivedSize + length - __packetSize;
            if (rest >= 0)
            {
                int recvLength = length - rest;
                Buffer.BlockCopy(readBuffer, offset, __receiveBuffer, __receivedSize, recvLength);
                __receivedSize += recvLength;

                if (__receivedSize == __packetSize)
                {
                    __receivedSize = __packetSize = 0;
                    SocketPacket socketPacket = new SocketPacket(__receiveBuffer);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(PacketProcess), socketPacket);
                }
            }
            else
            {
                Buffer.BlockCopy(readBuffer, offset, __receiveBuffer, __receivedSize, length);
                __receivedSize += length;
            }
        }

        private void OnClosed(object? sender, EventArgs e)
        {
        }

        private void OnError(object? sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

        }
    }
}
