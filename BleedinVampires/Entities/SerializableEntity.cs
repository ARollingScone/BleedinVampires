using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Entities
{
    [Serializable]
    class SerializableEntity
    {
        //World Coords as float[] instead of vector2f
        public float[] position;
        public float[] scale;
        public float rotation;

        int entityId;

        public SerializableEntity()
        {
            position = new float[2] { 0, 0 };
            scale = new float[2] { 1, 1 };
        }

        public PlayerEntity ToPlayerEntity()
        {
            PlayerEntity playerEntity = new PlayerEntity();

            playerEntity.position.X = position[0];
            playerEntity.position.Y = position[1];

            playerEntity.scale.X = scale[0];
            playerEntity.scale.Y = scale[1];

            playerEntity.rotation = rotation;

            return playerEntity;
        }
    }
}
