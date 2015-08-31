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
using System.Threading;

namespace BleedinVampires.Control
{
    class Client
    {
        UdpClient udpClient;
        IPEndPoint connectedServer;

        public int uniqueId;
        public bool bKeyStateChanged;
        public KeyState keyState;
        GameState currentGameState;

        public Client()
        {
            bKeyStateChanged = false;
            keyState = new KeyState();

            currentGameState = new GameState();

            udpClient = new UdpClient();
            connectedServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            udpClient.Connect(connectedServer);

            string msg = "Request Connection";
            var packet = new Packet<String>() { Payload = msg };
            var serializedData = packet.Serialize();
            udpClient.Send(serializedData, serializedData.Length);

            Thread recieveThread = new Thread(RecieveData);
            recieveThread.Start();

            Thread sendThread = new Thread(SendData);
            sendThread.Start();
        }

        public void SendData()
        {
            while(true)
            {
                if(bKeyStateChanged)
                {
                    var packet = new Packet<KeyState>() { Payload = keyState };
                    var serializedData = packet.Serialize();
                    udpClient.Send(serializedData, serializedData.Length);
                    bKeyStateChanged = false;
                }
            }
        }

        public void RecieveData()
        {
            while (true)
            {
                var data = udpClient.Receive(ref connectedServer);
                var deserializedData = Packet.Deserialize(data);
                
                if(deserializedData.PayloadType.Name == "GameState")
                {
                    this.currentGameState = (GameState) deserializedData.Payload;
                }
                else if(deserializedData.PayloadType.Name == "Int32")
                {
                    Console.WriteLine("Connected to server with Id: " + deserializedData.Payload);
                    this.uniqueId = (int)deserializedData.Payload;
                }
                else
                {
                    Console.WriteLine("Got am unexpected packet type of " + deserializedData.PayloadType.Name);
                }
            }
        }

        public GameState GetGameState()
        {
            GameState gs = currentGameState;
            return gs;
        }
    }
}


//byte[] data;            
//BinaryFormatter serializer = new BinaryFormatter();
//String connect = "connectme";            
//using(var ms = new MemoryStream()) 
//{
//    using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))
//    {
//        serializer.Serialize(ds, connect);
//    }
//    data = ms.ToArray();
//}
//udpClient.Send(data, data.Length);