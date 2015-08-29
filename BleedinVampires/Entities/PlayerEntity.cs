using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SFML using
using SFML.Graphics;

namespace BleedinVampires.Entities
{
    class PlayerEntity : DynamicEntity
    {
        public PlayerEntity()
        {
            position.X = 30;
            position.Y = 30;
            scale.X = 1;
            scale.Y = 1;
            rotation = 0;

            Image i = new Image(100, 100, Color.Green);
            texture = new Texture(i);
            sprite = new Sprite(new Texture(texture));
        }

        //Server Reads a client message and updates the entity
        public override void ServerUpdate_Entity(string message) { }

        //Client reads a server message and updates the entity
        public override void ClientUpdate_Entity(string message) { }

        //Converts the object into a packet string
        public override string ToPacketString()
        {
            string packetString = "";
            return packetString;
        }

        //Converts the object into a packet string with only changes since last send
        public override string ToPacketStringMinimal()
        {
            string packetString = "";
            return packetString;
        }
    }
}
