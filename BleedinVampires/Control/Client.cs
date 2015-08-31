using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using BleedinVampires.Entities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;

namespace BleedinVampires.Control
{
    class Client
    {
        MessagingSystem clientMessages;

        UdpClient udpClient;

        IPEndPoint connectedServer;

        public Client()
        {
            udpClient = new UdpClient();
            connectedServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            udpClient.Connect(connectedServer);
            //udpClient.Send(new byte[] { 1, 2, 3, 4, 5 }, 5);
            //udpClient.Send(new byte[] { 5, 4, 3, 2, 1 }, 5);
            //udpClient.Send(new byte[] { 1, 2, 3, 4, 5 }, 5);

            byte[] data;

            List<TestSerialize> testList = new List<TestSerialize>();

            for (int i = 0; i < 100; i++ )
            {
                testList.Add(new TestSerialize(i));
            }
                
            BinaryFormatter serializer = new BinaryFormatter();
            
            using(var ms = new MemoryStream()) 
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    serializer.Serialize(ds, testList);
                }
                data = ms.ToArray();
            }

            udpClient.Send(data, data.Length);

            // then receive data
            //var receivedData = udpClient.Receive(ref connectedServer);

            //Console.WriteLine("receive data from " + connectedServer.ToString());
        }
    }
}
