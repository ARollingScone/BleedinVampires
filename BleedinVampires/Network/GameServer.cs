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
    public class GameServer : GameNetwork
    {
        //List of Connected Clients
        public Dictionary<byte,IPEndPoint> Clients;

        public GameServer(int port = 8080)
        {
            this.port = port;

            bNetworkAlive = true;
            Clients = new Dictionary<byte, IPEndPoint>();

            //Create the server at this port
            this.UdpClient = new UdpClient(port);

            Inbox = new ConcurrentQueue<NetworkMessage>();
            Outbox = new ConcurrentQueue<NetworkMessage>();

            SendThread = new Thread(Send);
            ListenThread = new Thread(Listen);

            SendThread.Start();
            ListenThread.Start();
        }

        protected override void Send()
        {
            while(bNetworkAlive)
            {
                while(Outbox.Count > 0)
                {
                    NetworkMessage msg;
                    bool bMsg = Outbox.TryDequeue(out msg);

                    IPEndPoint destination;                    
                    bool bDest = Clients.TryGetValue(msg.NetworkDestination, out destination);

                    if (bMsg && bDest)
                    {
                        try
                        {
                            UdpClient.Send(msg.NetworkData, msg.NetworkData.Length, destination);
                        }
                        catch
                        {
                            System.Environment.Exit(0);
                        }
                    }
                }
            }
        }

        protected override void Listen()
        {
            while(bNetworkAlive)
            {
                try
                {
                    var endPoint = new IPEndPoint(IPAddress.Any, port);
                    var data = this.UdpClient.Receive(ref endPoint);

                    byte sender = CompareClients(endPoint);
                    if (sender != 0)
                    {
                        Inbox.Enqueue(new NetworkMessage(sender, data));
                    }
                    else
                    {
                        byte clientId = GetAvaliableClientId();
                        Clients.Add(clientId, endPoint);
                        Outbox.Enqueue(new NetworkMessage(clientId, CreateIdMessage(clientId)));

                        Console.WriteLine("Got new client at Id: " + clientId.ToString() + " with address " + endPoint.ToString());
                    }
                }
                catch(Exception e)
                {                   
                    Console.WriteLine(e.Message);
                    if (e.InnerException != null) Console.WriteLine(e.InnerException.Message);
                }
            }
        }

        //Returns the Id of the sending client
        public byte CompareClients(IPEndPoint NewClient)
        {
            foreach (var client in Clients) if (client.Value.ToString() == NewClient.ToString()) return client.Key;
            return 0;
        }

        //Returns next avaliable client id
        //Dictionary probably not read in order - change this later!!!
        public byte GetAvaliableClientId()
        {
            //Reserve 0 for the host
            byte i = 1;

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

        public byte[] CreateIdMessage(byte clientId)
        {
            byte[] msg = new byte[] {NetworkMessageType.ClientConfirmId, clientId};
            return msg;
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
