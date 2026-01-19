using System.Collections.Generic;
using BoomLib.Tools;
using Map.Rooms;
using UnityEngine;

namespace Map.MiniMap
{
    public class MinimapBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject renderTextureImage;
        [SerializeField] private Transform cameraTransform;

        [Space] 
        [SerializeField] private GameObject RoomPrefab;
        
        [Space] 
        [SerializeField] private Color currentRoomColor;
        [SerializeField] private Color visitedRoomColor;
        [SerializeField] private Color fogOfWarRoomColor;

        [Space]
        [SerializeField] private Vector2 cellSize;

        public static MinimapBuilder instance;

        private Dictionary<RoomPackage, SpriteRenderer> roomSprites = new Dictionary<RoomPackage, SpriteRenderer>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            RoomBuilder.OnBuildRooms.AddListener(BuildMinimap);
            renderTextureImage.SetActive(false);
        }
        
        public void UpdateMap()
        {
            List<RoomPackage> rooms = RoomBuilder.instance.Rooms;
            
            foreach (RoomPackage room in rooms)
            {
                roomSprites[room].color = ComputeRoomColor(room.hasBeenVisited);
                roomSprites[room].gameObject.SetActive(room.isVisible);
            }
            
            SetCurrentRoomColor();
            SetCameraPositionOnCurrentRoom();
        }

        public void SetMinimapState(bool state)
        {
            renderTextureImage.SetActive(state);
        }

        private void BuildMinimap()
        {
            List<RoomPackage> rooms = RoomBuilder.instance.Rooms;
            Vector2 offset = Vector2.one * (MapBuilder.instance.mapCenter * -1.0f);
            
            foreach (RoomPackage room in rooms)
            {
                SpriteRenderer roomSprite = SpawnRoom(room.x, room.y, offset, room.hasBeenVisited);
                roomSprites.Add(room, roomSprite);
                
                roomSprite.gameObject.SetActive(room.isVisible);
            }

            SetCurrentRoomColor();
            SetCameraPositionOnCurrentRoom();
        }

        private SpriteRenderer SpawnRoom(int x, int y, Vector2 offset, bool roomHasBeenVisited)
        {
            Vector2 position = transform.position.ToVector2() + new Vector2(x * cellSize.x, y * cellSize.y) + new Vector2(offset.x * cellSize.x, offset.y * cellSize.y);

            GameObject room = Instantiate(RoomPrefab, position, Quaternion.identity, transform);

            SpriteRenderer roomSprite = room.GetComponent<SpriteRenderer>();
            roomSprite.color = ComputeRoomColor(roomHasBeenVisited);

            return roomSprite;
        }
        
        private void SetCurrentRoomColor()
        {
            roomSprites[RoomBuilder.instance.CurrentRoom].color = currentRoomColor;
        }

        private Color ComputeRoomColor(bool roomHasBeenVisited)
        {
            return roomHasBeenVisited ? visitedRoomColor : fogOfWarRoomColor;
        }
        
        private void SetCameraPositionOnCurrentRoom()
        {
            Vector3 position = roomSprites[RoomBuilder.instance.CurrentRoom].transform.position;
            position.z = -10.0f;

            cameraTransform.transform.position = position;
        }
    }
}
