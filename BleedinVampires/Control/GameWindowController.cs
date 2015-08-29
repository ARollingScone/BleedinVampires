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
        bool bClosed = false;
        KeyState keyState;

        RenderWindow Window;
        View viewport;

        List<PlayerEntity> PlayerEntityList;

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
            keyState = new KeyState();

            PlayerEntityList = new List<PlayerEntity>();
            PlayerEntityList.Add(new PlayerEntity());

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear(Color.Black);
                Window.SetView(viewport);

                foreach(var player in PlayerEntityList)
                {
                    if (keyState.key_W) player.position.Y -= 0.1f;
                    if (keyState.key_A) player.position.X -= 0.1f;
                    if (keyState.key_S) player.position.Y += 0.1f;
                    if (keyState.key_D) player.position.X += 0.1f;
                    player.UpdateSprite();
                    Window.Draw(player.sprite);
                }                                

                Window.Display();
            }
        }

        void Event_OnClose(object sender, EventArgs e)
        {
            Console.WriteLine("Closing");
            //renderThread.Abort();
            Window.Close();
        }

        void Event_OnResize(object sender, EventArgs e)
        {
            ////Do we need to do this???
            //int resX = (int) ((SizeEventArgs)e).Width;
            //int resY = (int) ((SizeEventArgs)e).Height;

            //int newH = (800*resY)/resX;
            //int displace = (newH - 600)/(-2);

            //viewport = new View(new FloatRect(0, 0, resX, resY));
            //Console.WriteLine("Resizing");
        }

        void Event_OnKeyPush(object sender, EventArgs e)
        {
            KeyEventArgs keyArgs = (KeyEventArgs)e;
            Keyboard.Key key = keyArgs.Code;

            Console.WriteLine("Key Pressed: " + key.ToString());

            if (key == Keyboard.Key.W) keyState.key_W = true;
            if (key == Keyboard.Key.A) keyState.key_A = true;
            if (key == Keyboard.Key.S) keyState.key_S = true;
            if (key == Keyboard.Key.D) keyState.key_D = true;
        }

        void Event_OnKeyPop(object sender, EventArgs e)
        {
            KeyEventArgs keyArgs = (KeyEventArgs)e;
            Keyboard.Key key = keyArgs.Code;

            Console.WriteLine("Key Released: " + key.ToString());

            if (key == Keyboard.Key.W) keyState.key_W = false;
            if (key == Keyboard.Key.A) keyState.key_A = false;
            if (key == Keyboard.Key.S) keyState.key_S = false;
            if (key == Keyboard.Key.D) keyState.key_D = false;
        }

        void Event_OnLostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("Lost Focus");
            keyState.ClearAll();
        }
    }
}
