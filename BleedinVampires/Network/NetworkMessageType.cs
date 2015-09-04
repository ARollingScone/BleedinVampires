using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Network
{
    public static class NetworkMessageType
    {
        //Server Messages
        public const byte ServerSendId = 0;
        public const byte ServerWorldPacket = 1;
        public const byte ServerDisconnect = 2;

        //Client Messages
        public const byte ClientConnect = 10;
        public const byte ClientDisconnect = 11;
        public const byte ClientInput = 12;
        public const byte ClientConfirmId = 13;
       
    }
}
