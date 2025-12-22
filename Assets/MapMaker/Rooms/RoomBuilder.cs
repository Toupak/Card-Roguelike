using UnityEngine;

namespace MapMaker.Rooms
{
    public class RoomBuilder : MonoBehaviour
    {
        [SerializeField] private RoomDataBase roomDataBase;
        
        public static RoomBuilder instance;

        private MapBuilder mapBuilder;
        
        private int[][] map;

        private RoomData startingRoom = null;
        private RoomData currentRoom = null;
            
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
            return "";
        }

        public string GetCurrentRoom()
        {
            return currentRoom.roomName;
        }

        public string GetStartingRoom()
        {
            if (startingRoom == null)
                startingRoom = GetRoom(mapBuilder.mapCenter, mapBuilder.mapCenter);

            currentRoom = startingRoom;
            
            return startingRoom.roomName;
        }

        private RoomData GetRoom(int x, int y)
        {
            bool top = y > 0 && map[x][y - 1] != 0;
            bool right = x < mapBuilder.MapSize - 1 && map[x + 1][y] != 0;
            bool bot = y < mapBuilder.MapSize - 1 && map[x][y + 1] != 0;
            bool left = x > 0 && map[x - 1][y] != 0;
            
            return roomDataBase.GetRandomRoom(top, right, bot, left);
        }
    }
}
