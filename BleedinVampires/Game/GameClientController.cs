using BleedinVampires.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Game
{
    class GameClientController : GameController
    {
        GameClient gClient;

        public GameClientController()
        {
            this.inputManager = new Control.InputManager(1);

            //Setup game world
            this.gameWorld = new GameWorld();

            //Start the server
            this.gClient = new GameClient();            

            //Setup the Rendering loop
            SetupRenderingWindow();

            //Start the Rendering Loop
            RenderLoop();
        }

        public override void NetworkUpdate()
        {
            //Send our input to the server through the client

            //Create a deep copy clone so we don't mess with the thread
            ConcurrentQueue<NetworkMessage> messages = gClient.Inbox;

            //Snazzy
            NetworkMessage msg;
            while(messages.TryDequeue(out msg))
            {
                //Determine what the msg is and what to do with it
                if (msg.NetworkData[0] == NetworkMessageType.ServerWorldPacket)
                {
                    //Corresponds to the id in gServer clients and gameWorld inputs
                    byte msgId = msg.NetworkData[1];

                    gameWorld.FromNetworkData(msg.NetworkData);
                }
            }

            //If we need to notify the server our keys have changed
            if (bKeysChanged)
            {
                gClient.Outbox.Enqueue(new NetworkMessage(0, inputManager.ToNetworkData()));
                bKeysChanged = false;
            }
        }
    }
}
