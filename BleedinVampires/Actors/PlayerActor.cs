using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Actors
{
    public class PlayerActor : DynamicActor
    {
        #region MemberVariables

        //Member Variables
        public float[] position;
        public float rotation;

        #endregion

        public PlayerActor()
        {
            this.NetworkId = 12;
            this.position = new float[] { 15, 15 };
            this.rotation = 25;
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
    }
}
