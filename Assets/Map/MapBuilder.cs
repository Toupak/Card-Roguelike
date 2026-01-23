using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using Map.Floors;
using Map.Rooms;
using UnityEngine;
using UnityEngine.Events;
using Tools = BoomLib.Tools.Tools;

namespace Map
{
    public class MapBuilder : MonoBehaviour
    {
        private const int EMPTY = 0;
        private const int START = 1;
        private const int BOSS = 2;
        private const int COMBAT = 3;
        private const int ELITE = 4;
        private const int ENCOUNTER = 5;
        
        public static MapBuilder instance;
        
        public static UnityEvent OnBuildMap = new UnityEvent();
        
        private int[][] map;
        public int[][] Map => map;

        private int mapSize;
        public int MapSize => mapSize;
        public int mapCenter => MapSize / 2;

        private int currentRoomCount;
        public int RoomCount => currentRoomCount;
        private Queue<(int, int)> roomQueue;
        
        private int currentAlgoIterations;
        private const int maxAlgoIterations = 15000;

        private void Awake()
        {
            instance = this;
        }

        public void SetupMap(FloorData data, bool resetAlgo = true)
        {
            mapSize = data.floorSize;
            
            currentRoomCount = 0;
            roomQueue = new Queue<(int, int)>();
            if (resetAlgo)
                currentAlgoIterations = 0;

            ClearMap();

            VisitCell(mapCenter, mapCenter, data.maxRoomCount);
            map[mapCenter][mapCenter] = START;
            
            BuildMap(data);
        }

        private void ClearMap()
        {
            map = new int[mapSize][];
            for (int i = 0; i < mapSize; i++)
            {
                map[i] = new int[mapSize];
                for (int j = 0; j < mapSize; j++)
                    map[i][j] = EMPTY;
            }
        }

        private void BuildMap(FloorData data)
        {
            List<(int, int)> endRooms = new List<(int, int)>();

            while (roomQueue.Count > 0)
            {
                (int x, int y) = roomQueue.Dequeue();

                bool createdNeighbour = false;

                if (x > 0)
                    createdNeighbour |= VisitCell(x - 1, y, data.maxRoomCount);
                if (x < mapSize - 1)
                    createdNeighbour |= VisitCell(x + 1, y, data.maxRoomCount);
                if (y > 0)
                    createdNeighbour |= VisitCell(x, y - 1, data.maxRoomCount);
                if (y < mapSize - 1)
                    createdNeighbour |= VisitCell(x, y + 1, data.maxRoomCount);
                
                if (!createdNeighbour)
                    endRooms.Add((x, y));
            }

            int bossRoomIndex = SetupSpecialRooms(endRooms);
            int spawnedEliteCount = SetupEliteRooms(data, endRooms);
            int encounterRoomCount = LimitEncounterRooms(data, endRooms);
            
            bool isRoomCountValid = currentRoomCount >= data.minRoomCount;
            bool isBossRoomValid = bossRoomIndex >= 0;
            bool isEliteCountValid = spawnedEliteCount >= data.minEliteRooms && spawnedEliteCount <= data.maxEliteRooms;
            bool isEncounterCountValid = encounterRoomCount >= data.minEncounterRooms && encounterRoomCount <= data.maxEncounterRooms;

            bool isMapValid = isRoomCountValid && isBossRoomValid && isEliteCountValid && isEncounterCountValid;
            bool hasReachedMaxAlgoIterationCount = currentAlgoIterations >= maxAlgoIterations;
            if (!isMapValid && !hasReachedMaxAlgoIterationCount)
            {
                currentAlgoIterations += 1;
                SetupMap(data, false);
                return;
            }
            
            Debug.Log($"Map generated with {currentRoomCount} rooms, in {currentAlgoIterations + 1} iterations.");
            OnBuildMap?.Invoke();
        }
        
        private int LimitEncounterRooms(FloorData floorData, List<(int, int)> endRooms)
        {
            int maxEncounterRooms = floorData.maxEncounterRooms;

            List<(int, int)> encounterRooms = endRooms.Where((r) => Map[r.Item1][r.Item2] == ENCOUNTER).ToList();

            int encounterCount = encounterRooms.Count;
            if (encounterCount <= maxEncounterRooms)
                return encounterCount;
            
            encounterRooms.Shuffle();
            for (int i = encounterRooms.Count - 1; i >= 0 && i > maxEncounterRooms; i--)
            {
                Map[encounterRooms[i].Item1][encounterRooms[i].Item2] = COMBAT;
                encounterCount -= 1;
            }

            return encounterCount;
        }

        private int SetupEliteRooms(FloorData floorData, List<(int, int)> endRooms)
        {
            int eliteCount = Random.Range(floorData.minEliteRooms, floorData.maxEliteRooms + 1);

            List<(int, int)> encounterRooms = endRooms.Where((r) => Map[r.Item1][r.Item2] == ENCOUNTER).ToList();
            encounterRooms = encounterRooms.Where((keyPair) => ComputeDistanceFromCenter(keyPair.Item1, keyPair.Item2) >= floorData.eliteRoomMinDistanceFromStart).ToList();
            encounterRooms.Shuffle();

            int spawnedElite = 0;
            for (int i = 0; i < encounterRooms.Count && i < eliteCount; i++)
            {
                Map[encounterRooms[i].Item1][encounterRooms[i].Item2] = ELITE;
                spawnedElite += 1;
            }

            return spawnedElite;
        }

        private bool VisitCell(int x, int y, int maxRoomCount)
        {
            if (map[x][y] != 0)
                return false;

            if (GetNeighbourCount(x, y) > 1)
                return false;

            if (currentRoomCount >= maxRoomCount)
                return false;

            if (Tools.RandomBool())
                return false;

            roomQueue.Enqueue((x,y));
            map[x][y] = COMBAT;
            currentRoomCount += 1;

            return true;
        }

        private int GetNeighbourCount(int x, int y)
        {
            int count = 0;

            if (x > 0 && map[x - 1][y] > 0)
                count += 1;
            
            if (x < mapSize - 1 && map[x + 1][y] > 0)
                count += 1;

            if (y > 0 && map[x][y - 1] > 0)
                count += 1;
            
            if (y < mapSize - 1 && map[x][y + 1] > 0)
                count += 1;

            return count;
        }
        
        private int SetupSpecialRooms(List<(int, int)> endRooms)
        {
            float maxDistance = 0;
            int index = -1;
            for (int i = 0; i < endRooms.Count; i++)
            {
                (int x, int y) = endRooms[i];
                map[x][y] = ENCOUNTER;

                float distance = ComputeDistanceFromCenter(x, y);
                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    index = i;
                }
            }

            if (index >= 0)
            {
                (int x, int y) = endRooms[index];
                map[x][y] = BOSS;
            }
            
            return index;
        }

        private float ComputeDistanceFromCenter(int x, int y)
        {
            return Mathf.Sqrt(Mathf.Pow(x - mapCenter, 2) + Mathf.Pow(y - mapCenter, 2));
        }
        
        public RoomData.RoomType ComputeRoomType(int type)
        {
            if (type == START)
                return RoomData.RoomType.Starting;
            if (type == BOSS)
                return RoomData.RoomType.Boss;
            if (type == COMBAT)
                return RoomData.RoomType.Battle;
            if (type == ELITE)
                return RoomData.RoomType.Elite;
            if (type == ENCOUNTER)
                return RoomData.RoomType.Encounter;

            return RoomData.RoomType.Starting;
        }
    }
}
