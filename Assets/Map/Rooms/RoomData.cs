using UnityEngine;

namespace Map.Rooms
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
    public class RoomData : ScriptableObject
    {
        public enum DoorDirection
        {
            Top,
            Right,
            Bot,
            Left
        }
        
        //When Adding a new Value :
        // -> Got to RoomBuilder/RoomPackage if the new room is Hostile
        // -> Got to RoomBuilder.cs to assign it
        // -> Go to RoomFiller.cs to instantiate special stuff when entering room
        // -> Go to RunLoop.ComputeDifficulty() to prevent nullref
        public enum RoomType
        {
            Starting,
            Battle,
            Encounter,
            Elite,
            Boss
        }
        
        public string roomName;
        public string description;
        public string scenePath;

        [Space] 
        public Sprite screenShot;
        
        [Space]
        public bool hasTopDoor;
        public bool hasRightDoor;
        public bool hasBotDoor;
        public bool hasLeftDoor;

        public int DoorCount()
        {
            int count = 0;

            if (hasTopDoor)
                count += 1;

            if (hasRightDoor)
                count += 1;

            if (hasBotDoor)
                count += 1;

            if (hasLeftDoor)
                count += 1;

            return count;
        }

        public bool CheckDoors(bool t, bool r, bool b, bool l)
        {
            if (t != hasTopDoor)
                return false;

            if (r != hasRightDoor)
                return false;
            
            if (b != hasBotDoor)
                return false;
            
            if (l != hasLeftDoor)
                return false;

            return true;
        }
        
        public bool isIncomplete => DoorCount() < 1;
    }
}
