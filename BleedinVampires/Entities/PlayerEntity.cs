using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SFML using
using SFML.Graphics;

//
using BleedinVampires.Control;

namespace BleedinVampires.Entities
{
    [Serializable]
    class PlayerEntity : DynamicEntityBase
    {
        public int clientId;

        public PlayerEntity()
        {           
            position = new float[2];
            scale = new float[2];

            position[0] = 0;
            position[1] = 475;
            scale[0] = 0.25f;
            scale[1] = 0.25f;
            rotation = 0;
        }

        public override void Server_UpdateEntity(){}
    }
}
