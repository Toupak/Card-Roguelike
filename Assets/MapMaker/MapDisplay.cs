using BoomLib.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MapMaker
{
    public class MapDisplay : MonoBehaviour
    {
        [SerializeField] private Transform parentCanvas;
        [SerializeField] private Image roomPrefab;

        private MapBuilder mapBuilder;

        private void Start()
        {
            mapBuilder = GetComponent<MapBuilder>();
            MapBuilder.OnBuildMap.AddListener(DisplayMap);
        }

        private void DisplayMap()
        {
            parentCanvas.DeleteAllChildren();

            int[][] map = mapBuilder.Map;
            Vector2 offset = ComputeOffset();
            
            for (int i = 0; i < mapBuilder.MapSize; i++)
            {
                for (int j = 0; j < mapBuilder.MapSize; j++)
                {
                    if (map[i][j] > 0)
                        SpawnRoom(map, i, j, offset);
                }
            }
        }

        private Vector2 ComputeOffset()
        {
            float x = (1920.0f / 2.0f) - (mapBuilder.mapCenter * 100.0f);
            float y = (1080.0f / 2.0f) - (mapBuilder.mapCenter * 100.0f);

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
