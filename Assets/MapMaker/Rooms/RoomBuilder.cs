using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class RoomBuilder : MonoBehaviour
    {
        private class RoomPackage
        {
            public RoomData room;
            public int x;
            public int y;

            public RoomPackage(RoomData room, int x, int y)
            {
                this.room = room;
                this.x = x;
                this.y = y;
            }
        }
        
        [SerializeField] private RoomDataBase roomDataBase;
        
        public static RoomBuilder instance;

        private MapBuilder mapBuilder;
        
        private int[][] map;

        private RoomData currentRoom = null;
        private (int, int) currentRoomCoords;

        private List<RoomPackage> alreadyVisitedRooms = new List<RoomPackage>();
            
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mapBuilder = MapBuilder.instance;
            MapBuilder.OnBuildMap.AddListener(StoreMap);
        }

        private void StoreMap()
        {
            map = mapBuilder.Map;
        }

        public string GetNextRoom(RoomData.DoorDirection doorDirection)
        {
            (int x, int y) = ComputeNextRoomCoords(doorDirection);
            RoomData nextRoom = GetRoom(x, y);
            SetCurrentRoom(nextRoom, x, y);
            
            return nextRoom.roomName;
        }

        private (int, int) ComputeNextRoomCoords(RoomData.DoorDirection doorDirection)
        {
            int x = currentRoomCoords.Item1;
            int y = currentRoomCoords.Item2;
            
            switch (doorDirection)
            {
                case RoomData.DoorDirection.Top:
                    return (x, y - 1);
                case RoomData.DoorDirection.Right:
                    return (x + 1, y);
                case RoomData.DoorDirection.Bot:
                    return (x, y + 1);
                case RoomData.DoorDirection.Left:
                    return (x - 1, y);
                default:
                    throw new ArgumentOutOfRangeException(nameof(doorDirection), doorDirection, null);
            }
        }

        private void SetCurrentRoom(RoomData room, int x, int y)
        {
            currentRoom = room;
            currentRoomCoords = (x, y);
        }

        public string GetCurrentRoom()
        {
            return currentRoom.roomName;
        }

        public string GetStartingRoom()
        {
            RoomData room = GetRoom(mapBuilder.mapCenter, mapBuilder.mapCenter);

            SetCurrentRoom(room, mapBuilder.mapCenter, mapBuilder.mapCenter);
            
            return room.roomName;
        }

        private RoomData GetRoom(int x, int y)
        {
            Debug.Log("Get Room");
            
            foreach (RoomPackage roomPackage in alreadyVisitedRooms)
            {
                if (roomPackage.x == x && roomPackage.y == y)
                    return roomPackage.room;
            }

            return GetRoomFromDataBase(x, y);
        }

        private RoomData GetRoomFromDataBase(int x, int y)
        {
            Debug.Log("Get Room From DataBase");
            
            bool top = y > 0 && map[x][y - 1] != 0;
            bool right = x < mapBuilder.MapSize - 1 && map[x + 1][y] != 0;
            bool bot = y < mapBuilder.MapSize - 1 && map[x][y + 1] != 0;
            bool left = x > 0 && map[x - 1][y] != 0;
            
            RoomData newRoom = roomDataBase.GetRandomRoom(top, right, bot, left);
            
            alreadyVisitedRooms.Add(new RoomPackage(newRoom, x, y));

            return newRoom;
        }
    }
}
