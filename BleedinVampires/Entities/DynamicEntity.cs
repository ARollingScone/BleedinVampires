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
    abstract class DynamicEntity : EntityBase
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////Server Client Variables///////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        //World Positions
        public Vector2f prev_position;
        //World Scale
        public Vector2f prev_scale;
        //World Rotation
        public float prev_rotation;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////Server Client Methods/////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        //Server Reads a client message and updates the entity
        abstract public void ServerUpdate_Entity(string message);

        //Client reads a server message and updates the entity
        abstract public void ClientUpdate_Entity(string message);

        //Converts the object into a packet string
        abstract public string ToPacketString();

        //Converts the object into a packet string with only changes since last send
        abstract public string ToPacketStringMinimal();
    }
}
