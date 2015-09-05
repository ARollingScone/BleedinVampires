using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BleedinVampires.Network
{
    public struct NetworkMessage
    {
        public byte NetworkDestination;
        public byte[] NetworkData;

        public NetworkMessage(byte dest, byte[] data)
        {
            NetworkDestination = dest;
            NetworkData = data;
        }
    }

    public abstract class GameNetwork
    {
        protected int port;
        protected bool bNetworkAlive;

        protected UdpClient UdpClient;
        protected Thread SendThread;
        protected Thread ListenThread;

        public ConcurrentQueue<NetworkMessage> Outbox;
        public ConcurrentQueue<NetworkMessage> Inbox;

        protected abstract void Send();
        protected abstract void Listen();
    }
}
