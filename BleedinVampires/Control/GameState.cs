using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;


using BleedinVampires.Entities;


namespace BleedinVampires.Control
{
    //This class represents the state of the game
    //This is what the server sends to the client

    [Serializable]
    class GameState
    {
        //Contains a list of each type etc
        public Dictionary<int,PlayerEntity> Players;
        public int id;

        public GameState()
        {
            id = DateTime.Now.Second;
            Players = new Dictionary<int,PlayerEntity>();
        }
    }

    //De/Serializes the game state into a byte[] using Binary Formatter
    static class GameStateSerializer
    {
        public static byte[] ToPacket(GameState GS)
        {
            byte[] packet;
            BinaryFormatter serializer = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    serializer.Serialize(ds, GS);
                }
                packet = ms.ToArray();
            }
            return packet;
        }

        public static GameState FromPacket(byte[] packet)
        {
            GameState GS;
            BinaryFormatter serializer = new BinaryFormatter();

            using (var ms = new MemoryStream(packet))
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    GS = (GameState)serializer.Deserialize(ds);
                }
            }
            return GS;
        }  
    }
}
