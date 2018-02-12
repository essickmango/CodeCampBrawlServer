using System.Collections.Generic;
using UnityEngine;

namespace MainPlugin
{
   public class GameCreationData
   {
       public List<MapObject> MapObjects;
       public List<Vector2> SpawnPoints;

       public static GameCreationData CreateSimple(List<MapObject> mapObjects, List<Vector2> spawnPoints)
       {
           GameCreationData g = new GameCreationData();
           g.MapObjects = mapObjects;
           g.SpawnPoints = spawnPoints;

           return g;
       }
   }
}
