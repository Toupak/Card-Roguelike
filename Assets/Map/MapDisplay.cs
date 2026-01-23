using System;
using System.Collections.Generic;
using BoomLib.Tools;
using Map.Floors;
using Map.Rooms;
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
            RoomBuilder.OnBuildRooms.AddListener(DisplayMap);
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

            List<RoomPackage> map = RoomBuilder.instance.Rooms;
            int mapSize = MapBuilder.instance.MapSize;
            Vector2 offset = ComputeOffset(MapBuilder.instance.mapCenter);

            foreach (RoomPackage room in map)
            {
                SpawnRoom(room, offset);
            }
        }

        private Vector2 ComputeOffset(int mapCenter)
        {
            float x = (1920.0f / 2.0f) - (mapCenter * 100.0f);
            float y = (1080.0f / 2.0f) - (mapCenter * 100.0f);

            return new Vector2(x, y);
        }

        private void SpawnRoom(RoomPackage room, Vector2 offset)
        {
            float x = offset.x + 100.0f * room.x;
            float y = offset.y + 100.0f * room.y;
            Vector3 position = new Vector3(x, y);
            Image roomImage = Instantiate(roomPrefab, position, Quaternion.identity, parentCanvas);

            switch (room.roomType)
            {
                case RoomData.RoomType.Starting:
                    roomImage.color = Color.green;
                    break;
                case RoomData.RoomType.Battle:
                    break;
                case RoomData.RoomType.Encounter:
                    roomImage.color = Color.blue;
                    break;
                case RoomData.RoomType.Elite:
                    roomImage.color = Color.yellow;
                    break;
                case RoomData.RoomType.Boss:
                    roomImage.color = Color.red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
