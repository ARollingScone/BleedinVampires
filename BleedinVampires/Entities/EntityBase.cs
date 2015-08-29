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
    abstract class EntityBase
    {
        //World Positions
        public Vector2f position;
        //World Scale
        public Vector2f scale;
        //World Rotation
        public float rotation;

        //Drawing Features
        public Sprite sprite;
        public Texture texture;

        public void UpdateSprite()
        {
            sprite.Position = position;
            sprite.Scale = scale;
            sprite.Rotation = rotation;
        }
    }
}
