using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Control
{
    class KeyState
    {
        public bool key_A = false;
        public bool key_S = false;
        public bool key_W = false;
        public bool key_D = false;

        List<bool> keyList;

        public KeyState()
        {
            keyList = new List<bool>();

            keyList.Add(key_W);
            keyList.Add(key_A);
            keyList.Add(key_S);
            keyList.Add(key_D);
        }

        public void ClearAll()
        {
            for (int i = 0; i < keyList.Count; i++) keyList[i] = false;
        }
    }
}
