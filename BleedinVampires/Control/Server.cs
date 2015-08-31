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

        public ClientConnection(int id, IPEndPoint endPoint)
        {
            this.id = id;
            this.clientEndPoint = endPoint;
        }
    }

    class Server
    {
        MessagingSystem serverMessages;

        UdpClient udpServer;

        List<ClientConnection> connectedClients;

        public void testRecieve()
        {
            while (true)
            {
                var endPoint = new IPEndPoint(IPAddress.Any, 8080); 
                var data = udpServer.Receive(ref endPoint);
                Console.WriteLine("receive data from " + endPoint.ToString());
                //udpServer.Send(new byte[] { 1 }, 1, endPoint); // reply back

                //Check to see if we need to add a new client to the list of clients
                bool bFound = false;
                int highestId = 0;
                foreach(var client in connectedClients)
                {
                    if(client.id > highestId) highestId = client.id;
                    if (client.clientEndPoint.ToString() == endPoint.ToString()) bFound = true;
                }
                if(!bFound)
                {
                    highestId++;
                    connectedClients.Add(new ClientConnection(highestId, endPoint));
                    Console.WriteLine("Connected to new client at: " + endPoint.ToString());


                    List<TestSerialize> testList = new List<TestSerialize>();
                    BinaryFormatter serializer = new BinaryFormatter();

                    try
                    {
                        using (var ms = new MemoryStream(data))
                        {
                            using (var ds = new DeflateStream(ms, CompressionMode.Decompress, true))
                            {
                                testList = (List<TestSerialize>)serializer.Deserialize(ds);
                            }
                        }

                        if (testList != null)
                        {
                            foreach(var test in testList)
                            {
                                Console.WriteLine("Server: " + test.msg + " X: " + test.x + " Y:" + test.y);
                            }                            
                            Console.WriteLine("My packet size is: " + data.Length);
                         }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        if (e.InnerException != null) Console.WriteLine(e.InnerException.Message);
                    }

                }
                else
                {
                    Console.WriteLine("Already connected to client");
                }
            } 
        }

        public Server()
        {
            udpServer = new UdpClient(8080);
            connectedClients = new List<ClientConnection>();

            Thread recieve = new Thread(testRecieve);
            recieve.Start();            
        }
    }
}
