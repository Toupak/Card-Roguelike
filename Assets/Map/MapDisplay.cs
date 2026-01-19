using BoomLib.Tools;
using Map.Floors;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapDisplay : MonoBehaviour
    {
        [SerializeField] private FloorData floorData;
        [SerializeField] private Transform parentCanvas;
        [SerializeField] private Image roomPrefab;


        private void Start()
        {
            MapBuilder.OnBuildMap.AddListener(DisplayMap);
            MapBuilder.instance.SetupMap(floorData);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                MapBuilder.instance.SetupMap(floorData);
        }

        private void DisplayMap()
        {
            parentCanvas.DeleteAllChildren();

            int[][] map = MapBuilder.instance.Map;
            int mapSize = MapBuilder.instance.MapSize;
            Vector2 offset = ComputeOffset(MapBuilder.instance.mapCenter);
            
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i][j] > 0)
                        SpawnRoom(map, i, j, offset);
                }
            }
        }

        private Vector2 ComputeOffset(int mapCenter)
        {
            float x = (1920.0f / 2.0f) - (mapCenter * 100.0f);
            float y = (1080.0f / 2.0f) - (mapCenter * 100.0f);

            return new Vector2(x, y);
        }

        private void SpawnRoom(int[][] map, int i, int j, Vector2 offset)
        {
            float x = offset.x + 100.0f * i;
            float y = offset.y + 100.0f * j;
            Vector3 position = new Vector3(x, y);
            Image room = Instantiate(roomPrefab, position, Quaternion.identity, parentCanvas);
            
            if (map[i][j] == 2)
                room.color = Color.green;
            else if (map[i][j] == 3)
                room.color = Color.yellow;
            else if (map[i][j] == 4)
                room.color = Color.red;
        }
    }
}
