using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SFML References
using SFML.Graphics;
using SFML.Window;

//Local using
using BleedinVampires.Entities;

namespace BleedinVampires.Control
{
    class GameWindowController
    {
        KeyState keyState;
        KeyState prevKeyState;

        Server server;
        Client client;

        TextureManager TexManager;
        GameState GlobalGameState;

        RenderWindow Window;
        View viewport;

        public GameWindowController(int i)
        {
            TexManager = new TextureManager();
            GlobalGameState = new GameState();
            keyState = new KeyState();
            prevKeyState = new KeyState();

            if (i == 0)
            {
                server = new Server();
                GlobalGameState.Players.Add(0,new PlayerEntity());                
            }
            if (i == 1)
            {
                client = new Client();
            }
        }

        public void startRendering()
        {
            Window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(800, 600), "Bleedin' Vampires");
            Window.SetActive(true);

            Window.Closed += new EventHandler(Event_OnClose);
            Window.Resized += new EventHandler<SizeEventArgs>(Event_OnResize);
            Window.KeyPressed += new EventHandler<KeyEventArgs>(Event_OnKeyPush);
            Window.KeyReleased += new EventHandler<KeyEventArgs>(Event_OnKeyPop);
            Window.LostFocus += new EventHandler(Event_OnLostFocus);

            viewport = new View(new FloatRect(0, 0, 800, 600));
 
            DateTime lastSend = DateTime.Now;
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear(Color.Black);
                Window.SetView(viewport);

                foreach(var pvalue in GlobalGameState.Players)
                {
                    var player = pvalue.Value;

                    Window.Draw(player.getSprite(TexManager.LoadedTextures[1]));
                }

                //Perform Server Action
                if (server != null)
                {                   
                    foreach(var cc in server.connectedClients)
                    {
                        if(!GlobalGameState.Players.ContainsKey(cc.id))
                        {
                            PlayerEntity pe = new PlayerEntity();
                            pe.clientId = cc.id;
                            GlobalGameState.Players.Add(cc.id, pe);
                        }
                    }

                    foreach (var pvalue in GlobalGameState.Players)
                    {
                        var player = pvalue.Value;

                        if(player.clientId == 0)
                        {
                            if (keyState.key_W) player.position[1] -= 0.1f;
                            if (keyState.key_A) player.position[0] -= 0.1f;
                            if (keyState.key_S) player.position[1] += 0.1f;
                            if (keyState.key_D) player.position[0] += 0.1f;
                        }
                        else
                        {
                            foreach (var cc in server.connectedClients)
                            {
                                if (cc.id == player.clientId)
                                {
                                    if (cc.keyState.key_W) player.position[1] -= 0.1f;
                                    if (cc.keyState.key_A) player.position[0] -= 0.1f;
                                    if (cc.keyState.key_S) player.position[1] += 0.1f;
                                    if (cc.keyState.key_D) player.position[0] += 0.1f;
                                }
                            }
                        }
                    }

                    //Send the Current GameState when done
                    int diff = DateTime.Now.Subtract(lastSend).Milliseconds;
                    if (diff >= 34)
                    {
                        server.currentGameState = GlobalGameState;
                        lastSend = DateTime.Now;
                    }
                }
                //Perform Client Action
                else
                {
                    GlobalGameState = client.GetGameState();

                    if(!prevKeyState.Compare(keyState))
                    {
                        client.bKeyStateChanged = true;
                        client.keyState = keyState;
                        keyState.CopyTo(prevKeyState);
                    }
                }               

                Window.Display();
            }
        }

        void Event_OnClose(object sender, EventArgs e)
        {
            Window.Close();
        }

        void Event_OnResize(object sender, EventArgs e) {}

        void Event_OnKeyPush(object sender, EventArgs e)
        {
            KeyEventArgs keyArgs = (KeyEventArgs)e;
            Keyboard.Key key = keyArgs.Code;

            keyState.CopyTo(prevKeyState);
            if (key == Keyboard.Key.W) keyState.key_W = true;
            if (key == Keyboard.Key.A) keyState.key_A = true;
            if (key == Keyboard.Key.S) keyState.key_S = true;
            if (key == Keyboard.Key.D) keyState.key_D = true;
        }

        void Event_OnKeyPop(object sender, EventArgs e)
        {
            KeyEventArgs keyArgs = (KeyEventArgs)e;
            Keyboard.Key key = keyArgs.Code;

            keyState.CopyTo(prevKeyState);
            if (key == Keyboard.Key.W) keyState.key_W = false;
            if (key == Keyboard.Key.A) keyState.key_A = false;
            if (key == Keyboard.Key.S) keyState.key_S = false;
            if (key == Keyboard.Key.D) keyState.key_D = false;
        }

        void Event_OnLostFocus(object sender, EventArgs e) {}
    }
}
