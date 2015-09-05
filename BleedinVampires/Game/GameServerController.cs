using BleedinVampires.Actors;
using BleedinVampires.Control;
using BleedinVampires.Network;
using SFML.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Game
{
    class GameServerController : GameController
    {
        GameServer gServer;
        DateTime lastStateSent;

        //How often we send world state messages -> 1000ms / NumOfMessage
        // 50 = 20 Msg/s
        // 34 = 30 Msg/s
        // 17 = 60 Msg/s
        protected const int serverUpdateRate = 50;

        public GameServerController()
        {
            //Server is ID 0
            clientId = 0;

            //Setup the keyboard 
            this.inputManager = new InputManager(clientId);

            //Setup the gameworld
            this.gameWorld = new GameWorld();            

            //Start the server
            this.gServer = new GameServer();

            //Setup the Rendering loop
            SetupRenderingWindow();

            //Setup the first LastStateSent time for server messaging
            lastStateSent = DateTime.Now;

            //Start the Rendering Loop
            RenderLoop();
        }

        public override void NetworkUpdate()
        {
            lastStateSent = SendServerData(lastStateSent);

            //Create a deep copy clone so we don't mess with the thread
            ConcurrentQueue<NetworkMessage> messages = gServer.Inbox;

            //Snazzy
            NetworkMessage msg;
            while (messages.TryDequeue(out msg))
            {
                //Determine what the msg is and what to do with it
                if(msg.NetworkData[0] == NetworkMessageType.ClientInput)
                {
                    //Corresponds to the id in gServer clients and gameWorld inputs
                    byte msgId = msg.NetworkData[1];

                    InputManager input;
                    if(gameWorld.inputs.TryGetValue(msgId, out input))
                    {
                        //If we have the client update them
                        input.FromNetworkData(msg.NetworkData);
                    }
                    else
                    {
                        //If we don't, add them
                        input = new InputManager(msgId);
                        input.FromNetworkData(msg.NetworkData);
                        gameWorld.inputs.Add(msgId, input);
                    }
                }
            }
        }

        //Send Actor Data To Clients through server
        public DateTime SendServerData(DateTime lastSent)
        {
            //Send only if above min delay
            int diff = DateTime.Now.Subtract(lastSent).Milliseconds;
            if (diff >= serverUpdateRate)
            {
                //Create Outbox Messages

                byte[] data = gameWorld.ToNetworkData();
                
                foreach(var c in gServer.Clients)
                {
                    gServer.Outbox.Enqueue(new NetworkMessage(c.Key, data));
                }

                lastSent = DateTime.Now;
            }
            return lastSent;
        }
    }
}
