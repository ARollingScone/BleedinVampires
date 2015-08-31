using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Control
{
    [Serializable]
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

        public void CopyTo(KeyState ks)
        {
            ks.keyList = new List<bool>();
            
            ks.key_W = this.key_W;
            ks.key_A = this.key_A;
            ks.key_S = this.key_S;
            ks.key_D = this.key_D;

            ks.keyList.Add(ks.key_W);
            ks.keyList.Add(ks.key_A);
            ks.keyList.Add(ks.key_S);
            ks.keyList.Add(ks.key_D);
        }

        public bool Compare(KeyState ks)
        {
            return Enumerable.SequenceEqual(ks.keyList.OrderBy(t => t), this.keyList.OrderBy(t => t));         
        }

        public void ClearAll()
        {
            for (int i = 0; i < keyList.Count; i++) keyList[i] = false;
        }
    }
}
