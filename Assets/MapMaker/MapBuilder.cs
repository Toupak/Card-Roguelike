using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools = BoomLib.Tools.Tools;

namespace MapMaker
{
    public class MapBuilder : MonoBehaviour
    {
        [SerializeField] private int mapSize;
        [SerializeField] private int minRoomCount;
        [SerializeField] private int maxRoomCount;
        [SerializeField] private int maxAlgoIterations;

        public static UnityEvent OnBuildMap = new UnityEvent();
        
        private int[][] map;
        public int[][] Map => map;
        public int MapSize => mapSize;
        public int mapCenter => mapSize / 2;

        private Queue<(int, int)> roomQueue;
        private int currentRoomCount;
        private int algoIterations;

        private void Start()
        {
            SetupMap();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                algoIterations = 0;
                SetupMap();
            }
        }

        private void SetupMap()
        {
            currentRoomCount = 0;
            roomQueue = new Queue<(int, int)>();
            
            ClearMap();

            VisitCell(mapCenter, mapCenter);
            map[mapCenter][mapCenter] = 2;
            
            BuildMap();
        }

        private void ClearMap()
        {
            map = new int[mapSize][];
            for (int i = 0; i < mapSize; i++)
            {
                map[i] = new int[mapSize];
                for (int j = 0; j < mapSize; j++)
                    map[i][j] = 0;
            }
        }

        private void BuildMap()
        {
            List<(int, int)> endRooms = new List<(int, int)>();

            while (roomQueue.Count > 0)
            {
                (int x, int y) = roomQueue.Dequeue();

                bool createdNeighbour = false;

                if (x > 0)
                    createdNeighbour |= VisitCell(x - 1, y);
                if (x < mapSize - 1)
                    createdNeighbour |= VisitCell(x + 1, y);
                if (y > 0)
                    createdNeighbour |= VisitCell(x, y - 1);
                if (y < mapSize - 1)
                    createdNeighbour |= VisitCell(x, y + 1);
                
                if (!createdNeighbour)
                    endRooms.Add((x, y));
            }

            if (currentRoomCount < minRoomCount && algoIterations < maxAlgoIterations)
            {
                algoIterations += 1;
                SetupMap();
                return;
            }
            
            Debug.Log($"Map generated with {currentRoomCount} rooms, in {algoIterations + 1} iterations.");

            SetupSpecialRooms(endRooms);

            OnBuildMap?.Invoke();
        }

        private bool VisitCell(int x, int y)
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
            map[x][y] = 1;
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
        
        private void SetupSpecialRooms(List<(int, int)> endRooms)
        {
            float maxDistance = 0;
            int index = -1;
            for (int i = 0; i < endRooms.Count; i++)
            {
                (int x, int y) = endRooms[i];
                map[x][y] = 3;

                float distance = ComputeDistanceFromCenter(x, y);
                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    index = i;
                }
            }
            
            if (index < 0)
                Debug.LogWarning("Error no Boss room found");
            else
            {
                (int x, int y) = endRooms[index];
                map[x][y] = 4;
            }
        }

        private float ComputeDistanceFromCenter(int x, int y)
        {
            return Mathf.Sqrt(Mathf.Pow(x - mapCenter, 2) + Mathf.Pow(y - mapCenter, 2));
        }
    }
}
