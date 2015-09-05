using BleedinVampires.Actors;
using BleedinVampires.Control;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Game
{
    public abstract class GameController
    {
        #region Member Variables

        //Drawing / Rendering
        protected TextureManager textureManager;
        protected RenderWindow renderWindow;
        protected View view;

        //State / World
        protected DateTime lastFrame;
        protected GameWorld gameWorld;

        //Self Management
        protected bool bKeysChanged = false;
        protected InputManager inputManager;
        protected byte clientId;

        #endregion

        //Begin ren
        public virtual void SetupRenderingWindow()
        {
            renderWindow = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(800, 600), "Bleedin' Vampires");
            renderWindow.SetActive(true);

            renderWindow.Closed += new EventHandler(Event_OnClose);
            renderWindow.Resized += new EventHandler<SizeEventArgs>(Event_OnResize);
            renderWindow.KeyPressed += new EventHandler<KeyEventArgs>(Event_OnKeyPush);
            renderWindow.KeyReleased += new EventHandler<KeyEventArgs>(Event_OnKeyPop);
            renderWindow.LostFocus += new EventHandler(Event_OnLostFocus);

            view = new View(new FloatRect(0, 0, 800, 600));
        }

        public virtual void RenderLoop()
        {
            lastFrame = DateTime.Now;
            while (renderWindow.IsOpen)
            {
                //Assuming 60 fps
                float deltaTime = (float) DateTime.Now.Subtract(lastFrame).Milliseconds / 17;
                lastFrame = DateTime.Now;

                //Perform Network Methods
                NetworkUpdate();

                //Perform Updates on Dynamic Actors
                UpdateGame(deltaTime);

                //Dispatch Events and Clear the screen
                renderWindow.DispatchEvents();
                renderWindow.Clear(Color.Black);

                //Adjust and Set the view
                //Adjust view to player location
                renderWindow.SetView(view);

                //Render Each Drawable Actor
                DrawActors();

                //Draw frame on screen
                renderWindow.Display();       
            }
        }

        public virtual void UpdateGame(float deltaTime)
        {
            this.gameWorld.UpdateGameWorld(deltaTime);
        }

        public abstract void NetworkUpdate();

        public virtual void DrawActors()
        {
            foreach(var dActor in gameWorld.dynamicActors)
            {
                if (dActor.Value.bDrawable) dActor.Value.Draw(renderWindow);
            }
        }

        #region Events

        public virtual void Event_OnClose(object sender, EventArgs e)
        {
            renderWindow.Close();
        }

        public virtual void Event_OnResize(object sender, EventArgs e) { }

        public virtual void Event_OnKeyPush(object sender, EventArgs e)
        {
            KeyEventArgs keyArgs = (KeyEventArgs)e;
            Keyboard.Key key = keyArgs.Code;

            inputManager.KeyPress(key);
            bKeysChanged = true;
        }

        public virtual void Event_OnKeyPop(object sender, EventArgs e)
        {
            KeyEventArgs keyArgs = (KeyEventArgs)e;
            Keyboard.Key key = keyArgs.Code;

            inputManager.KeyRelease(key);
            bKeysChanged = true;
        }

        public virtual void Event_OnLostFocus(object sender, EventArgs e) { }

        #endregion
    }
}
