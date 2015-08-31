using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SFML using
using SFML.Graphics;
using SFML.System;

namespace BleedinVampires.Entities
{
    //Dynamic Entity Base is the base class for all enities being sent over the network
    //We don't send the previous data since it isn't relevant to the clients
    [Serializable]
    abstract class DynamicEntityBase
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////Server Client Variables///////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        //World Positions
        public float[] position;
        [NonSerialized] public float[] prev_position;

        //World Scale
        public float[] scale;
        [NonSerialized] public float[] prev_scale;

        //World Rotation
        public float rotation;
        [NonSerialized] public float prev_rotation;

        //Entity Id
        public int entityId;

        //Update the entity on the server
        abstract public void Server_UpdateEntity();

        //Draw Sprite Method
        public Sprite getSprite(Texture t)
        {
            Sprite s = new Sprite(t);

            s.Position = new Vector2f(position[0], position[1]);
            s.Scale = new Vector2f(scale[0], scale[1]);
            s.Rotation = rotation;

            return s;
        }
    }
}
