using BleedinVampires.Actors;
using BleedinVampires.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Game
{
    //Contains all the actors in the world
    public class GameWorld
    {
        #region Member Variables

        //Gamestate, main menu, in game, loading etc
        public enum GameState
        {
            InMenu,
            InGame,
            LoadLevel,
        }

        //Current GameState
        public GameState worldState;

        //Inputs
        public Dictionary<byte, InputManager> inputs;

        //Actors
        public List<Actor> staticActors;
        public Dictionary<int, DynamicActor> dynamicActors;

        #endregion

        public GameWorld()
        {
            inputs = new Dictionary<byte, InputManager>();

            worldState = GameState.InGame;

            dynamicActors = new Dictionary<int, DynamicActor>();
            staticActors = new List<Actor>();

            PlayerActor pa = new PlayerActor();
            dynamicActors.Add(1, pa);
        }

        //Decides how to update the world state
        public void UpdateGameWorld(float deltaTime)
        {
            switch(this.worldState)
            {
                case GameState.InGame: UpdateInGameState(deltaTime); break;
            }
        }

        //Update the ingame state
        public void UpdateInGameState(float deltaTime)
        {
            foreach(var dActor in dynamicActors)
            {
                //Update the dynamic actor
                dActor.Value.Update(deltaTime);

                //Test for World Collision
                foreach(var cActor in staticActors)
                {
                    if(GameWorldLogic.GetCanCollide(dActor.Value, cActor))
                    {
                        //Test Position
                    }
                }

                //Test for Dynamic Collision
                foreach(var cdActor in dynamicActors)
                {
                    if(cdActor.Key != dActor.Key && GameWorldLogic.GetCanCollide(dActor.Value, cdActor.Value))
                    {
                        //Test Position
                    }
                }
            }

            //Stops game freezing due to floats being too inaccurate at highspeed
            //System.Threading.Thread.Sleep(1);
        }

        //Convert to byte Array for Network
        public byte[] ToNetworkData()
        {
            IEnumerable<byte> networkData = new byte[] {Network.NetworkMessageType.ServerWorldPacket}; 

            foreach(var dActor in dynamicActors)
            {
                networkData = networkData.Concat(dActor.Value.GetNetworkData());
            }

            return networkData.ToArray();
        }

        public void FromNetworkData(byte[] networkData)
        {
            Console.Write("\nWorld Packet in: ");

            //Hardest part, all custom
            for(int i = 1; i < networkData.Length; i++)
            {
                Console.Write(networkData[i]);
            }

            PlayerActor pa = new PlayerActor();
            pa.SetNetworkData(networkData.Skip(1).ToArray());

            dynamicActors = new Dictionary<int, DynamicActor>();
            dynamicActors.Add(1, pa);
        }
    }

    public static class GameWorldLogic
    {
        //Test to see if it's possible for two actos to collide
        public static bool GetCanCollide(Actor actorA, Actor actorB)
        {
            switch(actorA.collideType)
            {
                case CollisionType.None: break;
                case CollisionType.World: break;
                case CollisionType.Powerup: break;
                case CollisionType.Player:
                {
                    if (actorB.collideType == CollisionType.World) return true;
                    if (actorB.collideType == CollisionType.Enemy) return true;
                    if (actorB.collideType == CollisionType.Powerup) return true;
                    break;
                }
                case CollisionType.Enemy:
                {
                    if (actorB.collideType == CollisionType.World) return true;
                    if (actorB.collideType == CollisionType.Player) return true;
                    break;
                }
            }

            return false;
        }

        //Test box collision between two actors
        public static bool GetCollide(Actor actorA, Actor actorB) 
        {
            float topA = actorA.position[1] - actorA.collisionSize[1];
            float botA = actorA.position[1] + actorA.collisionSize[1];
            float leftA = actorA.position[0] - actorA.collisionSize[0];
            float rightA = actorA.position[0] + actorA.collisionSize[0];

            float topB = actorA.position[1] - actorA.collisionSize[1];
            float botB = actorA.position[1] + actorA.collisionSize[1];
            float leftB = actorA.position[0] - actorA.collisionSize[0];
            float rightB = actorA.position[0] + actorA.collisionSize[0];

            //if (topA > botB) return false;
            //if (botA < topB) return false;
            //if (leftA > rightB) return false;
            //if (rightA < leftB) return false;

            return true;
        }

        //Test box collision between two actors and get the sides hit
        public static bool[] GetCollideSides(Actor actorA, Actor actorB)
        {
            bool[] sidesHit = new bool[4];

            return sidesHit;
        }
    }
}
