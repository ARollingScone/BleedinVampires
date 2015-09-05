using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Actors
{
    public enum CollisionType
    {
        None,
        World,
        Player,
        Enemy,
        Powerup
    }

    public abstract class Actor
    {
        #region MemberVariables

        //Member Variables
        public float[] position;
        public float rotation;

        #endregion

        //Drawing Details
        public bool bDrawable = false;
        public virtual void Draw(RenderWindow window) { }

        //Collision Details
        public CollisionType collideType = CollisionType.None;
        public int[] collisionSize;
    }
}
