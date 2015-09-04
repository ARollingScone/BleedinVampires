using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Actors
{
    //Dynamic Actors are replicated over the network
    //They are sent and received with:
    //      - GetNetworkData
    //      - SetNetworkData
    public abstract class DynamicActor : Actor
    {
        //The identifier of the dynamic actor
        public int NetworkId;

        //Convert the actor to network form
        public abstract byte[] GetNetworkData();

        //Convert the actor to a partial network form
        //public abstract byte[] GetNetworkDataPartial();

        //Recreate the actor from network form
        public abstract void SetNetworkData(byte[] NetworkData);

        //Modify the actor from network form
        //public abstract void SetNetworkDataPartial(byte[] NetworkData);
    }
}
