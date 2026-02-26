using System.Collections.Generic;
using Map.Encounters;
using UnityEngine;

namespace Map.Floors
{
    [CreateAssetMenu(fileName = "FloorData", menuName = "Scriptable Objects/FloorData")]
    public class FloorData : ScriptableObject
    {
        public int floorSize;
        public int minRoomCount;
        public int maxRoomCount;

        [Space] 
        public int minEliteRooms;
        public int maxEliteRooms;
        public int eliteRoomMinDistanceFromStart;
        
        [Space] 
        public int minEncounterRooms;
        public int maxEncounterRooms;
        
        [Space]
        public GameObject startingRoomEncounterPrefab;
        public List<EncounterPrefabData> mandatoryEncountersPrefabs;
        public List<EncounterPrefabData> optionalEncountersPrefabs;
    }
}
