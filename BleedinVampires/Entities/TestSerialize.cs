using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;

namespace BleedinVampires.Entities
{
    [Serializable]
    class TestSerialize
    {
        public int id;
        public string msg;

        public float x;
        public float y;

        public TestSerialize(int i)
        {
            Random r = new Random(i);
            id = r.Next(999);

            x = (float) (10 + r.Next(20)) / (1 + r.Next(10));
            y = (float)(10 + r.Next(20)) / (1 + r.Next(10));

            msg = "Hello World";

            string data = "T," + id + ",M," + msg + ",X," + x + ",Y," + y;

            //rotation = (float)(0 + r.Next(360)) / (1 + r.Next(10));

            //Console.WriteLine("My data size is: " + (System.Text.ASCIIEncoding.Unicode.GetByteCount(data).ToString()));
        }

    }
}
