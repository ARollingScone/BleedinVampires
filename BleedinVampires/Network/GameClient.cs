using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BleedinVampires.Network
{
    public class GameClient : GameNetwork
    {
        //The server we're connected to
        public IPEndPoint HostServer;

        public GameClient(string iPAddress = "127.0.0.1", int port = 8080)
        {
            this.port = port;
            bNetworkAlive = true;

            //Create the client at this port
            this.UdpClient = new UdpClient();
            this.HostServer = new IPEndPoint(IPAddress.Parse(iPAddress), port);
            
            this.UdpClient.Connect(HostServer);
            this.UdpClient.Send(new byte[] {NetworkMessageType.ClientConnect},1);

            Inbox = new ConcurrentQueue<NetworkMessage>();
            Outbox = new ConcurrentQueue<NetworkMessage>();

            SendThread = new Thread(Send);
            ListenThread = new Thread(Listen);

            SendThread.Start();
            ListenThread.Start();
        }

        protected override void Send()
        {
            while (bNetworkAlive)
            {
                NetworkMessage msg;
                while (Outbox.TryDequeue(out msg))
                {
                    UdpClient.Send(msg.NetworkData, msg.NetworkData.Length, HostServer);
                }
            }
        }

        protected override void Listen()
        {
            while (bNetworkAlive)
            {
                var data = this.UdpClient.Receive(ref HostServer);
                Inbox.Enqueue(new NetworkMessage(0, data));
            }
        }

        public void GetMessageType(byte[] NetworkData)
        {
            switch(NetworkData[0])
            {
                case NetworkMessageType.ServerSendId: Console.WriteLine("Got Id From Server"); break;
                case NetworkMessageType.ServerDisconnect: Console.WriteLine("Got Disconnect Msg from Server"); break;
                case NetworkMessageType.ServerWorldPacket: Console.WriteLine("Got World Information from Server"); break;
            }
        }
    }
}
