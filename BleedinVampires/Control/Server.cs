using BleedinVampires.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BleedinVampires.Control
{
    struct ClientConnection
    {
        public int id;
        public IPEndPoint clientEndPoint;
        public KeyState keyState;

        public ClientConnection(int id, IPEndPoint endPoint)
        {
            this.id = id;
            this.clientEndPoint = endPoint;
            this.keyState = new KeyState();
        }

        public void SetKeyState(KeyState ks)
        {
            ks.CopyTo(this.keyState);
        }
    }

    class Server
    {
        UdpClient udpServer;
        public List<ClientConnection> connectedClients;
        public GameState currentGameState;

        public Server()
        {
            currentGameState = new GameState();
            udpServer = new UdpClient(8080);
            connectedClients = new List<ClientConnection>();

            Thread recieve = new Thread(RecieveData);
            recieve.Start();

            Thread sendThread = new Thread(SendData);
            sendThread.Start();    
        }

        public void ConnectClient(IPEndPoint address)
        {
            bool bFound = false;
            int highestId = 0;
            ClientConnection CC;
            foreach (var client in connectedClients)
            {
                if (client.id > highestId) highestId = client.id;
                if (client.clientEndPoint.ToString() == address.ToString())
                {
                    bFound = true;
                    CC = client;
                }
            }
            if (!bFound)
            {
                highestId++;
                CC = new ClientConnection(highestId, address);
                connectedClients.Add(CC);
                Console.WriteLine("Client at " + address.ToString() + " connected");

                var packet = new Packet<int>() { Payload = CC.id };
                var serializedData = packet.Serialize();
                udpServer.Send(serializedData, serializedData.Length, CC.clientEndPoint);
            }
            else
            {
                Console.WriteLine("Error: Client already connected");
            }
        }

        public void RecieveData()
        {
            while (true)
            {
                var endPoint = new IPEndPoint(IPAddress.Any, 8080);
                var data = udpServer.Receive(ref endPoint);

                var deserializedData = Packet.Deserialize(data);
                if (deserializedData.PayloadType.Name == "String")
                {
                    if ((string)deserializedData.Payload == "Request Connection") ConnectClient(endPoint);                    
                }
                else if (deserializedData.PayloadType.Name == "KeyState")
                {
                    foreach(var client in connectedClients)
                    {
                        if (endPoint.ToString() == client.clientEndPoint.ToString())
                        {
                            client.SetKeyState((KeyState)deserializedData.Payload);
                            //Console.WriteLine("Got a keystate from Client " + client.id);
                            break;
                        }
                    }                    
                }
                else
                {
                    Console.WriteLine("Got am unexpected packet type of " + deserializedData.PayloadType.Name);
                }
            }
        }

        public void SendData()
        {
            while(true)
            {
                var packet = new Packet<GameState>() { Payload = currentGameState };
                var serializedData = packet.Serialize();

                ClientConnection[] clients = new ClientConnection[connectedClients.Count];
                connectedClients.CopyTo(clients);

                foreach(var client in clients)
                {
                    udpServer.Send(serializedData, serializedData.Length, client.clientEndPoint);
                }                
            }
        }
    }
}
