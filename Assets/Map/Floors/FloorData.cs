using UnityEngine;

namespace Map.Floors
{
    [CreateAssetMenu(fileName = "FloorData", menuName = "Scriptable Objects/FloorData")]
    public class FloorData : ScriptableObject
    {
        public int floorSize;
        public int minRoomCount;
        public int maxRoomCount;
    }
}
