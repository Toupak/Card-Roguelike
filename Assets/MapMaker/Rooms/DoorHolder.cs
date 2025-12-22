using System;
using System.Collections.Generic;
using BoomLib.Tools;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class DoorHolder : MonoBehaviour
    {
        public static DoorHolder instance;
        
        private List<DoorTrigger> doors = new List<DoorTrigger>();
    
        private void Awake()
        {
            instance = this;
            ComputeDoorList();
        }

        private void ComputeDoorList()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                doors.Add(transform.GetChild(i).GetChild(0).GetComponent<DoorTrigger>());
            }
        }

        public Vector3 GetDoorExitPosition(RoomData.DoorDirection doorDirection, float distance = 1.0f)
        {
            DoorTrigger door = GetDoorFromDirection(doorDirection);

            if (door == null)
                return Vector3.zero;

            return door.transform.position + (GetExitOffsetFromDirection(doorDirection) * distance);
        }

        private DoorTrigger GetDoorFromDirection(RoomData.DoorDirection doorDirection)
        {
            RoomData.DoorDirection opposite = doorDirection.Opposite();
            
            foreach (DoorTrigger door in doors)
            {
                if (door.DoorDirection == opposite)
                    return door;
            }

            return null;
        }
        
        private Vector3 GetExitOffsetFromDirection(RoomData.DoorDirection doorDirection)
        {
            switch (doorDirection)
            {
                case RoomData.DoorDirection.Top:
                    return Vector3.up;
                case RoomData.DoorDirection.Right:
                    return Vector3.right;
                case RoomData.DoorDirection.Bot:
                    return Vector3.down;
                case RoomData.DoorDirection.Left:
                    return Vector3.left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(doorDirection), doorDirection, null);
            }
        }
    }
}
