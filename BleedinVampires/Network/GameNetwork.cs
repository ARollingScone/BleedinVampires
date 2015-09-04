using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BleedinVampires.Network
{
    struct NetworkMessage
    {
        public int NetworkDestination;
        public byte[] NetworkData;

        public NetworkMessage(int dest, byte[] data)
        {
            NetworkDestination = dest;
            NetworkData = data;
        }
    }

    public abstract class GameNetwork
    {
        protected bool bNetworkAlive;

        protected UdpClient UdpClient;
        protected Thread SendThread;
        protected Thread ListenThread;

        public Queue<NetworkMessage> Outbox;
        public Queue<NetworkMessage> Inbox;

        protected abstract void Send();
        protected abstract void Listen();
    }
}
