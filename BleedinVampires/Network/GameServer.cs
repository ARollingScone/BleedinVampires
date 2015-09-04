using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BleedinVampires.Network
{
    public class GameServer : GameNetwork
    {
        int port;

        //List of Connected Clients
        public Dictionary<int,IPEndPoint> Clients;

        public GameServer(int port = 8080)
        {
            this.port = port;

            bNetworkAlive = true;
            Clients = new Dictionary<int, IPEndPoint>();

            //Create the server at this port
            this.UdpClient = new UdpClient(port);

            Inbox = new Queue<NetworkMessage>();
            Outbox = new Queue<NetworkMessage>();

            SendThread = new Thread(Send);
            ListenThread = new Thread(Listen);

            SendThread.Start();
            ListenThread.Start();
        }

        public override void Send()
        {
            while(bNetworkAlive)
            {
                while(Outbox.Count > 0)
                {
                    NetworkMessage msg = Outbox.Dequeue();
                    IPEndPoint destination;
                    
                    Clients.TryGetValue(msg.NetworkDestination, out destination);

                    if(destination != null) UdpClient.Send(msg.NetworkData, msg.NetworkData.Length, destination);
                }
            }
        }

        public override void Listen()
        {
            while(bNetworkAlive)
            {
                var endPoint = new IPEndPoint(IPAddress.Any, port);
                var data = this.UdpClient.Receive(ref endPoint);

                GetMessageType(data);

                int sender = CompareClients(endPoint);
                if(sender != 0)
                {
                    Inbox.Enqueue(new NetworkMessage(sender, data));
                }
                else
                {
                    int clientId = GetAvaliableClientId();
                    Clients.Add(clientId, endPoint);                  
                    Outbox.Enqueue(new NetworkMessage(clientId, CreateIdMessage(clientId)));
                }
            }
        }

        //Returns the Id of the sending client
        public int CompareClients(IPEndPoint NewClient)
        {
            foreach (var client in Clients) if (client.Value.Address.ToString() == NewClient.Address.ToString()) return client.Key;
            return 0;
        }

        //Returns next avaliable client id
        //Dictionary probably not read in order - change this later!!!
        public int GetAvaliableClientId()
        {
            //Reserve 0 for the host
            int i = 1;

            foreach(var client in Clients)
            {
                if (i == client.Key)
                {
                    i++;
                }
                else
                {
                    return i;
                }
            }
            return i;
        }

        public byte[] CreateIdMessage(int clientId)
        {
            byte[] id = BitConverter.GetBytes(clientId);
            byte[] msgType = new byte[] {NetworkMessageType.ClientConfirmId};

            IEnumerable<byte> message = msgType.Concat(id);

            return message.ToArray();
        }

        public void GetMessageType(byte[] NetworkData)
        {
            switch(NetworkData[0])
            {
                case NetworkMessageType.ClientConnect: Console.WriteLine("Client asking to connect"); break;
                case NetworkMessageType.ClientConfirmId: Console.WriteLine("Client confirmed Id"); break;
                case NetworkMessageType.ClientDisconnect: Console.WriteLine("Client disconnected"); break;
                case NetworkMessageType.ClientInput: Console.WriteLine("Client sent their input"); break;
            }
        }
    }
}
