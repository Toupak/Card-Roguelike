using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Map.Rooms
{
    public class PointOfInterest
    {
        public GameObject prefab;
        public Sprite icon;
        public Vector3 position;
        public bool removeOnCleared;

        public PointOfInterest(GameObject prefab, Sprite icon, Vector3 position, bool removeOnCleared)
        {
            this.prefab = prefab;
            this.icon = icon;
            this.position = position;
            this.removeOnCleared = removeOnCleared;
            
            if (icon != null)
                Debug.Log("Zuzu : icon is not null : " + icon.name);
        }
    }
    
    public class RoomPackage
    {
        public RoomData room;
        public int x;
        public int y;
        public RoomData.RoomType roomType;
        public bool isVisible;
        public bool hasBeenVisited;
        public bool hasBeenCleared;
        
        public List<PointOfInterest> pointOfInterests = new List<PointOfInterest>();

        public bool IsHostile => roomType == RoomData.RoomType.Battle || roomType == RoomData.RoomType.Elite || roomType == RoomData.RoomType.Boss;
        
        public RoomPackage(RoomData room, int x, int y, RoomData.RoomType roomType)
        {
            this.room = room;
            this.x = x;
            this.y = y;
            this.roomType = roomType;
            this.isVisible = roomType == RoomData.RoomType.Starting;
            this.hasBeenVisited = roomType == RoomData.RoomType.Starting;
            this.hasBeenCleared = roomType == RoomData.RoomType.Starting;
        }

        public void AddPointOfInterest(GameObject prefab, Sprite icon, Vector3 position, bool removeOnCleared)
        {
            pointOfInterests.Add(new PointOfInterest(prefab, icon, position, removeOnCleared));
        }
    }
    
    public class RoomBuilder : MonoBehaviour
    {
        [SerializeField] private RoomDataBase roomDataBase;

        public static UnityEvent OnBuildRooms = new UnityEvent();
        
        public static RoomBuilder instance;

        private int mapSize;
        
        private RoomPackage currentRoom = null;
        public RoomPackage CurrentRoom => currentRoom;
        private (int, int) currentRoomCoords => (currentRoom.x, currentRoom.y);
        private RoomData.RoomType currentRoomType => currentRoom.roomType;

        private List<RoomPackage> rooms = new List<RoomPackage>();
        public List<RoomPackage> Rooms => rooms;
            
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            MapBuilder.OnBuildMap.AddListener(BuildRooms);
        }

        private void BuildRooms()
        {
            int[][] map = MapBuilder.instance.Map;
            mapSize = MapBuilder.instance.MapSize;
            
            rooms = new List<RoomPackage>();
            
            for (int x = 0; x < mapSize; x++)
                for (int y = 0; y < mapSize; y++)
                    if (map[x][y] != 0)
                        rooms.Add(new RoomPackage(GetRoomFromDataBase(map, x, y), x, y, MapBuilder.instance.ComputeRoomType(map[x][y])));
            
            ComputeStartingRoom();
            
            OnBuildRooms?.Invoke();
        }

        public RoomPackage ComputeStartingRoom()
        {
            currentRoom = GetRoom(RoomData.RoomType.Starting);
            UpdateRoomsVisibility();
            return currentRoom;
        }

        public RoomPackage ComputeNextRoom(RoomData.DoorDirection doorDirection)
        {
            (int x, int y) = ComputeNextRoomCoords(doorDirection);
            currentRoom = GetRoom(x, y);
            currentRoom.hasBeenVisited = true;
            UpdateRoomsVisibility();
            return currentRoom;
        }
        
        public string GetCurrentRoomName()
        {
            return currentRoom.room.roomName;
        }
        
        public RoomData.RoomType GetCurrentRoomType()
        {
            return currentRoomType;
        }

        public bool HasRoomBeenCleared()
        {
            if (currentRoom != null)
                return currentRoom.hasBeenCleared;

            return true;
        }

        public void MarkCurrentRoomAsCleared()
        {
            if (currentRoom != null)
                currentRoom.hasBeenCleared = true;
        }
        
        private void UpdateRoomsVisibility()
        {
            currentRoom.hasBeenVisited = true;
            currentRoom.isVisible = true;
            
            (int x, int y) = ComputeNextRoomCoords(RoomData.DoorDirection.Left);
            RoomPackage room = GetRoom(x, y);
            if (room != null)
                room.isVisible = true;
            
            (x, y) = ComputeNextRoomCoords(RoomData.DoorDirection.Top);
            room = GetRoom(x, y);
            if (room != null)
                room.isVisible = true;
            
            (x, y) = ComputeNextRoomCoords(RoomData.DoorDirection.Right);
            room = GetRoom(x, y);
            if (room != null)
                room.isVisible = true;
            
            (x, y) = ComputeNextRoomCoords(RoomData.DoorDirection.Bot);
            room = GetRoom(x, y);
            if (room != null)
                room.isVisible = true;
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

        private RoomPackage GetRoom(int x, int y)
        {
            foreach (RoomPackage room in rooms)
            {
                if (room.x == x && room.y == y)
                    return room;
            }

            return null;
        }
        
        private RoomPackage GetRoom(RoomData.RoomType roomType)
        {
            foreach (RoomPackage room in rooms)
            {
                if (room.roomType == roomType)
                    return room;
            }

            return null;
        }

        private RoomData GetRoomFromDataBase(int[][] map, int x, int y)
        {
            bool top = y > 0 && map[x][y - 1] != 0;
            bool right = x < mapSize - 1 && map[x + 1][y] != 0;
            bool bot = y < mapSize - 1 && map[x][y + 1] != 0;
            bool left = x > 0 && map[x - 1][y] != 0;
            
            return roomDataBase.GetRandomRoom(top, right, bot, left);
        }
    }
}
