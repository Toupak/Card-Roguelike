using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Rooms
{
    [CreateAssetMenu(fileName = "RoomDataBase", menuName = "Scriptable Objects/RoomDataBase")]
    public class RoomDataBase : ScriptableObject
    {
        [SerializeField] private List<RoomData> allRooms = new List<RoomData>();
        public IReadOnlyList<RoomData> AllRooms => allRooms;

        public void InitializeEmptyList()
        {
            if (allRooms == null)
                allRooms = new List<RoomData>();
        }

        public RoomData GetByDoorCount(int doorCount, bool force = false)
        {
            if (!force && doorCount <= 0) 
                return null;
            return allRooms.Find(c => c != null && c.DoorCount() == doorCount);
        }

        public RoomData GetByName(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
                return null;
            return allRooms.Find(c => c != null && string.Equals(c.roomName, roomName, StringComparison.Ordinal));
        }

        public RoomData GetRandomRoom(bool top, bool right, bool bot, bool left, Predicate<RoomData> predicate = null)
        {
            List<RoomData> pool = allRooms.FindAll(c => c != null && c.CheckDoors(top, right, bot, left));
        
            pool = (predicate == null)
                ? pool.FindAll(c => c != null)
                : pool.FindAll(c => c != null && predicate(c));

            if (pool.Count == 0) 
                return null;

            return pool[UnityEngine.Random.Range(0, pool.Count)];
        }

        public List<RoomData> GetAllRooms(Predicate<RoomData> predicate = null)
        {
            List<RoomData> pool = (predicate == null)
                ? allRooms.FindAll(c => c != null)
                : allRooms.FindAll(c => c != null && predicate(c));

            if (pool.Count == 0) 
                return null;

            return pool;
        }

        public void Sort(Comparison<RoomData> comparison)
        {
            allRooms.Sort(comparison);
        }
    }
}
