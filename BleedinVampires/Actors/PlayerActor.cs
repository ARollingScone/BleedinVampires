using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

namespace BleedinVampires.Actors
{
    public class PlayerActor : DynamicActor
    {
        public PlayerActor()
        {
            this.NetworkId = 12;
            this.position = new float[] { 15, 15 };
            this.rotation = 25;
            this.bDrawable = true;

            this.collideType = CollisionType.Player;
            this.collisionSize = new int[] { 60, 60 };
        }

        #region GetNetwork

        //Convert the actor to network form
        public override byte[] GetNetworkData()
        {
            byte[] networkIden = BitConverter.GetBytes(NetworkId);
            byte[] positionX = BitConverter.GetBytes(position[0]);
            byte[] positionY = BitConverter.GetBytes(position[1]);
            byte[] rotationA = BitConverter.GetBytes(rotation);

            IEnumerable<byte> networkData = networkIden.Concat(positionX).Concat(positionY).Concat(rotationA);

            return networkData.ToArray();
        }

        //Convert the actor to a partial network form
        //public abstract byte[] GetNetworkDataPartial();

        #endregion

        #region SetNetwork

        //Recreate the actor from network form
        public override void SetNetworkData(byte[] NetworkData)
        {
            Console.WriteLine("\nDescribe Player With Id: " + BitConverter.ToInt32(NetworkData, 0) + "\n");
            Console.WriteLine("Position X: " + BitConverter.ToSingle(NetworkData, 4));
            Console.WriteLine("Position Y: " + BitConverter.ToSingle(NetworkData, 8));
            Console.WriteLine("Rotation: " + BitConverter.ToSingle(NetworkData, 12));

            this.position[0] = BitConverter.ToSingle(NetworkData, 4);
            this.position[1] = BitConverter.ToSingle(NetworkData, 8);
            this.rotation = BitConverter.ToSingle(NetworkData, 12);
        }

        //Modify the actor from network form
        //public abstract void SetNetworkDataPartial(byte[] NetworkData);

        #endregion

        public override void Draw(RenderWindow window)
        {
            CircleShape shape = new CircleShape(30);
            shape.Position = new Vector2f(position[0], position[1]);
            window.Draw(shape);
        }

        public override void Update(float deltaTime)
        {
            position[0] += (1 * deltaTime);
            position[1] += (0.75f * deltaTime);
        }
    }
}
