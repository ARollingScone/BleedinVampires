using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Control
{
    [Serializable]
    public class Packet
    {
        public Type PayloadType { get; protected set; }
        public object Payload { get; protected set; }
        public static Packet Deserialize(byte[] bytes)
        {
            Packet p = new Packet();
            BinaryFormatter bf = new BinaryFormatter();
            using(var m = new MemoryStream(bytes))
            {
                using(var ds = new DeflateStream(m, CompressionMode.Decompress, true))
                {
                    p = (Packet)bf.Deserialize(ds);
                }
            }

            return p;
        }
    }

    [Serializable]
    class Packet<T> : Packet
    {
        public Packet()
        {
            PayloadType = typeof(T);
        }
        public new T Payload
        {
            get { return (T)base.Payload; }
            set { base.Payload = value; }
        }

        public override string ToString()
        {
            return "[Packet]" + Payload.ToString();
        }

        public byte[] Serialize()
        {
            //MemoryStream m = new MemoryStream();
            //DeflateStream ds = new DeflateStream(m, CompressionMode.Compress, true);
            //(new BinaryFormatter()).Serialize(ds, this);
            //return m.ToArray();

            BinaryFormatter serializer = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    serializer.Serialize(ds, this);
                }
                return ms.ToArray();
            }
        }
    }
}
